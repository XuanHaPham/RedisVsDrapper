using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace RedisVsDrapperDemo.Model.Repository
{
    public interface IUnitOfWork :IDisposable
    {
        Guid Id { get; }
        IDbConnection Connection { get; }
        IDbTransaction Transaction { get; }
        IDbTransaction BeginNewTran();
        void Begin();
        void Commit();
        void Rollback();
        void Commit(IDbTransaction Transaction);
        void Rollback(IDbTransaction Transaction);
    }
}
