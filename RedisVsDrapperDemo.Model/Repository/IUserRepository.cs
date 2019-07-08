using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RedisVsDrapperDemo.Model.Repository
{
    public interface IUserRepository: IRepository
    {
        Task<User> Get(string Username);
    }
}
