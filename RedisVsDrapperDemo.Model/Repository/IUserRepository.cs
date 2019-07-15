using RedisVsDrapperDemo.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RedisVsDrapperDemo.Model.Repository
{
    public interface IUserRepository: IRepository<User>
    {
        Task<User> Get(string Username);
        Task AddUser(UserViewModel user);
    }
}
