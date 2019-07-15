using RedisVsDrapperDemo.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RedisVsDrapperDemo.Model.Service
{
    public interface IUserService : IService
    {
        Task<UserDTO> Login(string Username, string Password);
        Task AddUserAsync(UserViewModel user);
    }
}
