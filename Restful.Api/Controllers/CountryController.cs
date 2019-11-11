using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restful.Api.Resourses;
using Restful.Core.Services;
using Restful.Core;
using Restful.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restful.Api.Controllers
{
    [Route("api/[controller]/{id?}")]
    public class CountryController:ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ICountryRepository countryRepository;
        private readonly IUnitOfWork unitOfWork;

        public CountryController(IMapper mapper,
            ICountryRepository countryRepository,IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.countryRepository = countryRepository;
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var countries = await countryRepository.GetCountriesAync();

            var countryResource = mapper.Map<List<CountryResource>>(countries);
            return Ok(countryResource);
        }
    }
}
