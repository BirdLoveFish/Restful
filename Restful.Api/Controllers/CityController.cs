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
        private readonly IUrlHelper urlHelper;

        public CityController(IMapper mapper,
            ICountryRepository countryRepository, IUnitOfWork unitOfWork,
            ICityRepository cityRepository,IUrlHelper urlHelper)
        {
            this.mapper = mapper;
            this.countryRepository = countryRepository;
            this.unitOfWork = unitOfWork;
            this.cityRepository = cityRepository;
            this.urlHelper = urlHelper;
        }

        [HttpGet(Name = "GetCitiesForCountry")]
        public async Task<IActionResult> GetCitiesForCountry(Guid countryId)
        {
            if(!await countryRepository.CountryExists(countryId))
            {
                return NotFound();
            }

            var cities = await cityRepository.GetCitiesAsync(countryId);
            var citiesResource = mapper.Map<IEnumerable<CityResource>>(cities);
            citiesResource = citiesResource.Select(CreateLinksFactory);
            var wrapper = new LinkCollectionResourceWrapper<CityResource>(citiesResource);

            return Ok(CreateLinksForCities(wrapper));
        }


        [HttpGet("{cityId}",Name = "GetCity")]
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
            return Ok(CreateLinksFactory(cityResource));
        }

        [HttpPost(Name = "AddCity")]
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
                new { countryId, cityId = cityModel.Id }, CreateLinksFactory(cityResource));

        }

        [HttpDelete("{cityId}",Name = "DeleteCity")]
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

        [HttpPut("{cityId}",Name = "UpdateCity")]
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

        [HttpPatch("{cityId}",Name = "UpdatePatialCity")]
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

        private CityResource CreateLinksFactory(CityResource city)
        {
            city.Links.Add(new LinkResource(
                urlHelper.Link("GetCity", new { city.CountryId, cityId = city.Id }),
                "self","GET"));
            city.Links.Add(new LinkResource(
                urlHelper.Link("DeleteCity", new {city.CountryId, cityId = city.Id }),
                "delete_city", "DELETE"));
            city.Links.Add(new LinkResource(
                urlHelper.Link("UpdateCity", new { city.CountryId, cityId = city.Id }),
                "update_city", "PUT"));
            city.Links.Add(new LinkResource(
                urlHelper.Link("UpdatePatialCity", new { city.CountryId, cityId = city.Id }),
                "update_patial_city", "PATCH"));
            return city;
        }

        private LinkCollectionResourceWrapper<CityResource> CreateLinksForCities(
            LinkCollectionResourceWrapper<CityResource> citiesWrapper)
        {
            citiesWrapper.Links.Add(
                new LinkResource(urlHelper.Link("GetCitiesForCountry", null), 
                "self", "GET"));
            return citiesWrapper;
        }

    }
}
