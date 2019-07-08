using RedisVsDrapperDemo.Model;
using RedisVsDrapperDemo.Model.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RedisVsDrapperDemo.Repository
{
    public class UserRepository : IUserRepository
    {
        public async Task<User> Get(string Username)
        {
            return new User {
                UserId = Guid.NewGuid(),
                Username = "test",
                Password = "123456",
                Permission = PermissionEnum.INSERT
            };
        }
    }
}
