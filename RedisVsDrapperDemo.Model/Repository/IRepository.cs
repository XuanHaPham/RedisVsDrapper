using System;
using System.Collections.Generic;
using System.Text;

namespace RedisVsDrapperDemo.Model.Repository
{
    public interface IRepository
    {
        IUnitOfWork UnitOfWork { get; }
    }
    public interface IRepository<TEntity>:IRepository where TEntity : class
    {
        TEntity Get(int Id);
        IEnumerable<TEntity> GetAll();
        void Add(TEntity entity);
        void Delete(TEntity entity);
        void Update(TEntity entity);
    }
}
