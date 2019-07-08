using System.Threading.Tasks;

namespace RedisVsDrapperDemo.Model.Repository
{
    public interface IRedisRepository : IRepository
    {
        Task<T> Get<T>(string HashKey, string HashField);
        Task<bool> Push<T>(T Model, string HashKey, string HashField);
        Task<bool> Delete(string HashKey, string HashField);
    }
}
