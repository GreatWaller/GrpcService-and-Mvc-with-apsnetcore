using GrpcService.Models;
using GrpcService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class GreeterController:ControllerBase
    {

        private IUserService _userService;

        public GreeterController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]AuthenticateModel model)
        {
            var user = _userService.Authenticate(model.Username, model.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(new {name="Hi"});
        }
    }
}
