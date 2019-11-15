using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Restful.Core;
using Restful.Core.Models;
using Restful.Core.Services;
using Restful.Infrastructure.Resourses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restful.Api.Controllers
{
    [Route("api/country/{countryId}/city")]
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

        [HttpPost]
        public async Task<IActionResult> AddCity(Guid countryId,[FromBody]CityAddViewModel city)
        {
            if(city == null)
            {
                return BadRequest();
            }

            if(!await countryRepository.CountryExists(countryId))
            {
                return NotFound();
            }

            var cityModel = mapper.Map<City>(city);
            await cityRepository.AddCityForCountry(countryId, cityModel);
            if(!await unitOfWork.SaveAsync())
            {
                return StatusCode(500, "server error");
            }
            var cityResource = mapper.Map<CityResource>(cityModel);
            return CreatedAtAction(nameof(GetCity), 
                new { countryId, cityId = cityModel.Id }, cityResource);

        }

        [HttpDelete("{cityId}")]
        public async Task<IActionResult> DeleteCity(Guid countryId,Guid cityId)
        {
            if (!await countryRepository.CountryExists(countryId))
            {
                return NotFound();
            }

            var cityModel = await cityRepository.GetCityForCountry(countryId, cityId);
            if(cityModel == null)
            {
                return NotFound();
            }

            await cityRepository.DeleteCityForCountry(cityModel);
            if (!await unitOfWork.SaveAsync())
            {
                return StatusCode(500);
            }

            return NoContent();
        }

        [HttpPut("{cityId}")]
        public async Task<IActionResult> UpdateCity(Guid countryId, Guid cityId,
            [FromBody]CityUpdateViewModel city)
        {
            if(city == null)
            {
                return BadRequest();
            }
            if(!await countryRepository.CountryExists(countryId))
            {
                return NotFound();
            }
            var cityModel = await cityRepository.GetCityForCountry(countryId, cityId);
            if (cityModel == null)
            {
                cityModel = mapper.Map<City>(city);
                cityModel.Id = cityId;
                await cityRepository.AddCityForCountry(countryId, cityModel);
                if(!await unitOfWork.SaveAsync())
                {
                    return StatusCode(500);
                }
                //return NotFound();
                return CreatedAtAction(nameof(GetCity), new { countryId, cityId }, cityModel);
            }
            mapper.Map(city, cityModel);
            await cityRepository.UpdateCity(cityModel);
            if(!await unitOfWork.SaveAsync())
            {
                return StatusCode(500);
            }
            return NoContent();
        }

        [HttpPatch("{cityId}")]
        public async Task<IActionResult> UpdatePatialCity(Guid countryId, Guid cityId,
            [FromBody]JsonPatchDocument<CityUpdateViewModel> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }
            if (!await countryRepository.CountryExists(countryId))
            {
                return NotFound();
            }
            var cityModel = await cityRepository.GetCityForCountry(countryId, cityId);
            if (cityModel == null)
            {
                return NotFound();
            }
            var cityUpdate = mapper.Map<CityUpdateViewModel>(cityModel);
            patchDoc.ApplyTo(cityUpdate);
            mapper.Map(cityUpdate, cityModel);

            await cityRepository.UpdateCity(cityModel);
            if (!await unitOfWork.SaveAsync())
            {
                return StatusCode(500);
            }
            return NoContent();
        }

    }
}
