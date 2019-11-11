using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Restful.Api.Resourses;
using Restful.Core;
using Restful.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restful.Api.Controllers
{
    [Route("api/countries/{countryId}/cities")]
    public class CityController:ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ICountryRepository countryRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICityRepository cityRepository;

        public CityController(IMapper mapper,
            ICountryRepository countryRepository, IUnitOfWork unitOfWork,
            ICityRepository cityRepository)
        {
            this.mapper = mapper;
            this.countryRepository = countryRepository;
            this.unitOfWork = unitOfWork;
            this.cityRepository = cityRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetCitiesForCountry(Guid countryId)
        {
            if(!await countryRepository.CountryExists(countryId))
            {
                return NotFound();
            }

            var cities = await cityRepository.GetCitiesAsync(countryId);
            var citiesResource = mapper.Map<List<CityResource>>(cities);
            return Ok(citiesResource);
        }

        [HttpGet("{cityId}")]
        public async Task<IActionResult> GetCity(Guid countryId, Guid cityId)
        {
            if (!await countryRepository.CountryExists(countryId))
            {
                return NotFound();
            }

            var city = await cityRepository.GetCityForCountry(countryId,cityId);
            if(city == null)
            {
                return NotFound();
            }

            var cityResource = mapper.Map<CityResource>(city);
            return Ok(cityResource);
        }



    }
}
