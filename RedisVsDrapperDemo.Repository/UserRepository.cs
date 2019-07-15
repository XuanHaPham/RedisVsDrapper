using Dapper;
using RedisVsDrapperDemo.Model;
using RedisVsDrapperDemo.Model.Repository;
using RedisVsDrapperDemo.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace RedisVsDrapperDemo.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {

        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        public async Task<IEnumerable<User>> GetUsersById(int UserId)
        {
            var query = "usp_GetAllBlogPostByPageIndex";
            var param = new DynamicParameters();
            var list = await UnitOfWork.Connection.QueryAsync<User>(query, param, commandType: CommandType.StoredProcedure);
            return list;
        }
        public async Task AddUser(UserViewModel user)
        {
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@UserName", user.UserName);
            dynamicParameters.Add("@Email", user.Email);
            dynamicParameters.Add("@Password", user.Password);

            await UnitOfWork.Connection.ExecuteAsync(
                "spAddUser",
                dynamicParameters,
                commandType: CommandType.StoredProcedure);
        }
        public async Task<User> Get(string Username) =>
            await UnitOfWork.Connection.QueryFirstAsync<User>(
                $"select * from [{nameof(User)}] with (NOLOCK) where [{nameof(User.Username)}] = @{nameof(Username)}",
                new { Username });
    }
}
