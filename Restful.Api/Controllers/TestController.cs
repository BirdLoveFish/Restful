using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restful.Api.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restful.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly TestContainer testContainer;

        public TestController()
        {
        }

        [HttpGet]
        public IActionResult Get()
        {
            var username = User.Claims.First(x => x.Type == "email").Value;
            return Ok(username);
            //return new JsonResult(from c in User.Claims select new { c.Type, c.Value});
        }
    }
}
