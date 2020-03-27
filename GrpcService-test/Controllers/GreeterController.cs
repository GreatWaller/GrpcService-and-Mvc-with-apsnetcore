using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GreeterController:ControllerBase
    {
        [HttpGet]
        [Route("all")]
        public IActionResult GetAll()
        {
            return Ok(new {name="Hi"});
        }
    }
}
