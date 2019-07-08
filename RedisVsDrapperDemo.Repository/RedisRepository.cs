using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;
using RedisVsDapperDemo.Helper;
using RedisVsDrapperDemo.Model.Repository;

namespace RedisVsDrapperDemo.Repository
{
    public class RedisRepository : IRedisRepository
    {
        internal class RedisConnection
        {
            public string Connection { get; set; }
            public string Server { get; set; }
            public int DBNumber { get; set; }
        }
        /// <summary>
        /// Apply singleton pattern for ConnectionMultiplexer to save CPU usage
        /// </summary>
        private static readonly RedisConnection ConnectionModel = ConfigHelper.Get<RedisConnection>("RedisConnection");
        private static Lazy<ConnectionMultiplexer> Single = new Lazy<ConnectionMultiplexer>(() => CreateConnection());
        public static ConnectionMultiplexer CreateConnection()
        {
            var connection = ConnectionMultiplexer.Connect(ConnectionModel.Connection);
            connection.PreserveAsyncOrder = false;
            return connection;
        }
        public static IDatabase Database = InitDB(Single.Value, ConnectionModel.DBNumber);
        public static IServer Server = InitServer(Single.Value, ConnectionModel.Server);
        //public IUnitOfWork UnitOfWork => throw new NotImplementedException();

        public static IServer InitServer(ConnectionMultiplexer redis, string Server)
        {
            return redis.GetServer(Server);
        }
        public static IDatabase InitDB(ConnectionMultiplexer Redis, int DataBaseNum = 0)
        {
            return Redis.GetDatabase(DataBaseNum);
        }

        public async Task<T> Get<T>(string HashKey, string HashField)
        {
            try
            {
                var result = await Database.HashGetAsync(HashKey, HashField);
                if (!result.HasValue) return default(T);
                return JsonConvert.DeserializeObject<T>(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> Push<T>(T Model, string HashKey, string HashField)
        {
            try
            {
                return await Database.HashSetAsync(HashKey, HashField, JsonConvert.SerializeObject(Model));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> Delete(string HashKey, string HashField)
        {
            try
            {
                return await Database.HashDeleteAsync(HashKey, HashField);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
