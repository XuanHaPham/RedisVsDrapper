
using RedisVsDrapperDemo.Model.Repository;
using System;
using System.Data;
using System.Threading.Tasks;

namespace RedisVsDapperDemo.Helper
{
    public static class RepositoryExtension
    {
        public async static Task Transaction(this IRepository Repository, Func<Task> action)
        {
            if (action == null || Repository.UnitOfWork == null) return;
            var UnitOfWork = Repository.UnitOfWork;
            Exception exception = null;
            using (DalSession dalSession = new DalSession(UnitOfWork))
            {
                UnitOfWork.Begin();
                try
                {
                    await action();
                    UnitOfWork.Commit();
                }
                catch (Exception ex)
                {
                    UnitOfWork.Rollback();
                    exception = ex;
                }
            }
            if (exception != null) throw new Exception("Query DB failed!", exception);
        }
        public async static Task<T> Transaction<T>(this IRepository Repository, Func<Task<T>> action)
        {
            if (action == null || Repository.UnitOfWork == null) return default(T);
            var UnitOfWork = Repository.UnitOfWork;
            T result;
            Exception exception = null;
            using (DalSession dalSession = new DalSession(UnitOfWork))
            {
                UnitOfWork.Begin();
                try
                {
                    result = await action();
                    UnitOfWork.Commit();
                }
                catch(Exception ex)
                {
                    UnitOfWork.Rollback();
                    result = default(T);
                    exception = ex;
                }
            }
            if (exception != null) throw new Exception("Query DB failed!", exception);
            return result;
        }
        public async static Task MannualTransaction(this IRepository Repository, Func<Task> action)
        {
            if (action == null || Repository.UnitOfWork == null) return;
            var UnitOfWork = Repository.UnitOfWork;
            Exception exception = null;
            using (DalSession dalSession = new DalSession(UnitOfWork))
            {
                try
                {
                    await action();
                }
                catch (Exception ex)
                {
                    UnitOfWork.Rollback();
                    exception = ex;
                }
            }
            if (exception != null) throw new Exception("Query DB failed!", exception);
        }
        public async static Task<T> MannualTransaction<T>(this IRepository Repository, Func<Task<T>> action)
        {
            if (action == null || Repository.UnitOfWork == null) return default(T);
            var UnitOfWork = Repository.UnitOfWork;
            T result;
            Exception exception = null;
            using (DalSession dalSession = new DalSession(UnitOfWork))
            {
                try
                {
                    result = await action();
                }
                catch (Exception ex)
                {
                    UnitOfWork.Rollback();
                    result = default(T);
                    exception = ex;
                }
            }
            if (exception != null) throw new Exception("Query DB failed!", exception);
            return result;
        }
    }
    internal sealed class DalSession : IDisposable
    {
        internal DalSession(IUnitOfWork UnitOfWork)
        {
            this.UnitOfWork = UnitOfWork;
            _connection = UnitOfWork.Connection;
            _connection.Open();
        }

        IDbConnection _connection = null;
        public IUnitOfWork UnitOfWork { get; private set; }
        public void Dispose()
        {
            UnitOfWork.Dispose();
            _connection.Dispose();
        }
        
    }

}
