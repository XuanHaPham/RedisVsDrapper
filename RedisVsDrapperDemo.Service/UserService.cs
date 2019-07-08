using RedisVsDrapperDemo.Model;
using RedisVsDrapperDemo.Model.Repository;
using RedisVsDrapperDemo.Model.Service;
using System.Threading.Tasks;

namespace RedisVsDrapperDemo.Service
{
    public class UserService : IUserService
    {
        private static readonly string USER_REDIS_KEY = "USER";
        private static readonly string USER_IDENTITY_KEY = "Identity";
        public UserService(IUserRepository UserRepo, IRedisRepository RedisRepo)
        {
            this.UserRepo = UserRepo;
            this.RedisRepo = RedisRepo;
        }
        private IUserRepository UserRepo { get; set; }
        private IRedisRepository RedisRepo { get; set; }
        public async Task<UserDTO> Login(string Username, string Password)
        {
            var user = await UserRepo.Get(Username);
            if (user == null) return null;
            if (user.Password != Password) return null;
            if (user.Permission == PermissionEnum.NONE) return null;
            var result = new UserDTO {
                UserId = user.UserId,
                Username = user.Username,
                Permission = user.Permission
            };
            await RedisRepo.Push(result, $"{USER_REDIS_KEY}:{result.UserId}", USER_IDENTITY_KEY);
            return result;
        }

    }
}
