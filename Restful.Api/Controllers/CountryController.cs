using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restful.Api.Resourses;
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
        private readonly IMapper mapper;

        public CountryController(MyContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var countries = await context.Countries.ToListAsync();

            var countryResource = mapper.Map<List<CountryResource>>(countries);
            return Ok(countryResource);
        }
    }
}
