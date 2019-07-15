using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedisVsDrapperDemo.Model.Service;
using RedisVsDrapperDemo.Model.ViewModel;

namespace RedisVsDrapperDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _service;
        public UserController(IUserService userService)
        {
            _service = userService;
        }
        [HttpPost]
        public async Task Post([FromBody]UserViewModel user)
        {
            await _service.AddUserAsync(user);
        }
    }
}