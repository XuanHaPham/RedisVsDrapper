using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RedisVsDrapperDemo.Model.Service;

namespace RedisVsDrapperDemo.Controllers
{
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    [Route("api/[controller]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        private IUserService UserService { get; set; }
        public MainController(IUserService userService)
        {
            UserService = userService;
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<string> Login(LoginModel model)
        {
            if (model == null ||
                string.IsNullOrWhiteSpace(model.Username) ||
                string.IsNullOrWhiteSpace(model.Password))
                return null;
            var result = await UserService.Login(model.Username, model.Password);
            if (result == null) return null;
            return JsonConvert.SerializeObject(result);
        }
    }
}