using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restful.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restful.Api.Controllers
{
    [Route("api/[controller]/{id?}")]
    public class CountryController:ControllerBase
    {
        private readonly MyContext context;

        public CountryController(MyContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await context.Countries.ToListAsync());
        }
    }
}
