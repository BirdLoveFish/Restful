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
using Restful.Core.Models;
using Microsoft.AspNetCore.Http;

namespace Restful.Api.Controllers
{
    [Route("api/[controller]")]
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
            var countries = await countryRepository.GetCountriesAsync();

            var countryResource = mapper.Map<List<CountryResource>>(countries);
            return Ok(countryResource);
        }

        [HttpGet("{id?}")]
        public async Task<IActionResult> GetCountry(Guid id)
        {
            var country = await countryRepository.GetCountryById(id);
            if(country == null)
            {
                return NotFound();
            }

            var countryResource = mapper.Map<CountryResource>(country);
            return Ok(countryResource);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CountryAddViewModel countryAddViewModel)
        {
            var country = mapper.Map<Country>(countryAddViewModel);
            
            await countryRepository.AddCountryAsync(country);
            if(!await unitOfWork.SaveAsync())
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error when Add");
            }

            var countryResource = mapper.Map<CountryResource>(country);

            return CreatedAtAction(nameof(GetCountry),new { id = countryResource.Id}, countryResource);
        }
    }
}
