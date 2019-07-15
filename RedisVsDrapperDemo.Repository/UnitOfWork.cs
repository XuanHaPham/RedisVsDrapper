using RedisVsDrapperDemo.Model.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace RedisVsDrapperDemo.Repository
{
    //public static class StringBuilderExtensions
    //{
    //    public static StringBuilder AppendSpace(this StringBuilder stringBuilder, string value) =>
    //        stringBuilder.Append($" {value}");
    //    public static StringBuilder AppendSpace(this StringBuilder stringBuilder, StringBuilder value) =>
    //        stringBuilder.Append($" {value}");
    //}
    public sealed class UnitOfWork : IUnitOfWork
    {
        public static IUnitOfWork Create(IDbConnection connection)
        {
            return new UnitOfWork(connection);
        }
        internal UnitOfWork(IDbConnection connection)
        {
            _id = Guid.NewGuid();
            Connection = connection;
            LazyTransaction = new Lazy<IDbTransaction>(() =>
            {
                _transaction = Connection.BeginTransaction();
                return _transaction;
            });
        }

        public IDbConnection Connection { get; set; }
        public IDbTransaction Transaction { get; set; }
        IDbTransaction _transaction = null;
        Guid _id = Guid.Empty;

        IDbConnection IUnitOfWork.Connection
        {
            get { return Connection; }
        }
        IDbTransaction IUnitOfWork.Transaction
        {
            get { return _transaction; }
        }
        Guid IUnitOfWork.Id
        {
            get { return _id; }
        }
        private Lazy<IDbTransaction> LazyTransaction { get; set; }
        public void Begin()
        {
            var temp = LazyTransaction.Value;
        }
        public IDbTransaction BeginNewTran() =>
            Connection.BeginTransaction();
        public void Commit()
        {
            Commit(_transaction);
            _transaction = null;
        }

        public void Rollback()
        {
            Rollback(_transaction);
            _transaction = null;
        }
        public void Commit(IDbTransaction Transaction)
        {
            if (Transaction != null)
            {
                Transaction.Commit();
                Transaction.Dispose();
            }
        }

        public void Rollback(IDbTransaction Transaction)
        {
            if (Transaction != null)
            {
                Transaction.Rollback();
                Transaction.Dispose();
            }
        }

        public void Dispose()
        {
            if (_transaction != null)
                _transaction.Dispose();
            _transaction = null;
        }

    }
}
