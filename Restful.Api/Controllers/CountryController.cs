using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restful.Core.Services;
using Restful.Core;
using Restful.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Restful.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Restful.Infrastructure.Extensions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using Restful.Api.Extensions;
using Newtonsoft.Json;
using Restful.Infrastructure.Resourses;

namespace Restful.Api.Controllers
{
    [Route("api/[controller]")]
    public class CountryController:ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ICountryRepository countryRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<CountryController> logger;
        private readonly IUrlHelper urlHelper;

        public CountryController(IMapper mapper,
            ICountryRepository countryRepository,IUnitOfWork unitOfWork,
            ILogger<CountryController> logger,IUrlHelper urlHelper)
        {
            this.mapper = mapper;
            this.countryRepository = countryRepository;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
            this.urlHelper = urlHelper;
        }

        [HttpGet(Name ="GetCountries")]
        public async Task<IActionResult> Get(CountryResourceParameters parameters)
        {
            var countries = await countryRepository.GetCountriesAsync(parameters);
            var countryResource = mapper.Map<List<CountryResource>>(countries);

            var preLink = countries.HasPrevious
                ? CreateCountryUri(parameters, PaginationResourceUriType.PreviousPage) : null;

            var nextLink = countries.HasNext
                ? CreateCountryUri(parameters, PaginationResourceUriType.NextPage) : null;

            var meta = new
            {
                countries.TotalItemsCount,
                countries.PaginationBase.PageIndex,
                countries.PaginationBase.PageSize,
                countries.PageCount,
                PreviousLink = preLink,
                NextLink = nextLink
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(meta));

            return Ok(countryResource);
        }

        [HttpGet("{id}")]
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

        [HttpPost("{id}")]
        public async Task<IActionResult> BlockCreatingCountry(Guid id)
        {
            var country = await countryRepository.GetCountryById(id);
            if (country == null)
            {
                return NotFound();
            }

            return StatusCode(StatusCodes.Status409Conflict);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CountryAddViewModel country)
        {
            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResultCus(ModelState);
            }

            var countryModel = mapper.Map<Country>(country);
            
            await countryRepository.AddCountryAsync(countryModel);
            if(!await unitOfWork.SaveAsync())
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error when Add");
            }

            var countryResource = mapper.Map<CountryResource>(countryModel);

            return CreatedAtAction(nameof(GetCountry),new { id = countryResource.Id}, countryResource);
        }

        [HttpPost("collection")]
        public async Task<IActionResult> AddCountriesCollection([FromBody]List<CountryAddViewModel> countries)
        {
            var countriesModel = mapper.Map<List<Country>>(countries);
            await countryRepository.AddCountriesCollection(countriesModel);

            if (!await unitOfWork.SaveAsync())
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error when Add");
            }

            var countriesResource = mapper.Map<List<CountryResource>>(countriesModel);
            var idsStr = string.Join(',', countriesResource.Select(a => a.Id));

            return CreatedAtAction(nameof(GetCountryFromIds), new { ids = idsStr }, countriesResource);
        }

        [HttpGet("collection/({ids})")]
        public async Task<IActionResult> GetCountryFromIds(
            [ModelBinder(BinderType =typeof(CusArrayModelBinder))]IEnumerable<Guid> ids)
        {
            if(ids == null)
            {
                return BadRequest();
            }

            var countriesModel = await countryRepository.GetCountriesAsync(ids);
            if (countriesModel.Count() != ids.Count())
            {
                return NotFound();
            }
            var countriesResource = mapper.Map<List<CountryResource>>(countriesModel);
            
            return Ok(countriesResource);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var country = await countryRepository.GetCountryById(id);
            if(country == null)
            {
                return NotFound();
            }
            await countryRepository.DeleteCountry(country);
            if(!await unitOfWork.SaveAsync())
            {
                return StatusCode(500);
            }
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdatePatialCity(Guid id,
            [FromBody]JsonPatchDocument<CountryUpdateViewModel> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }



            var countryModel = await countryRepository.GetCountryById(id);
            if (countryModel == null)
            {
                return NotFound();
            }
            var countryUpdate = mapper.Map<CountryUpdateViewModel>(countryModel);
            patchDoc.ApplyTo(countryUpdate, ModelState);
            TryValidateModel(countryUpdate);
            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            mapper.Map(countryUpdate, countryModel);

            await countryRepository.UpdateCountry(countryModel);
            if (!await unitOfWork.SaveAsync())
            {
                return StatusCode(500);
            }
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCountry(Guid id,
            [FromBody]CountryUpdateViewModel country)
        {
            //if(country == null)
            //{
            //    return BadRequest();
            //}

            //if (!ModelState.IsValid)
            //{
            //    return new UnprocessableEntityObjectResult(ModelState);
            //}

            //var countryModel = await countryRepository.GetCountryById(id,true);
            //if(countryModel == null)
            //{
            //    return NotFound();
            //}

            ////所有的id
            //var countryUpdateCityIds = country.Cities.Select(a => a.Id).ToList();
            ////remove
            //var removeCities = countryModel.Cities
            //    .Where(a => !countryUpdateCityIds.Contains(a.Id)).ToList();
            //foreach (var city in removeCities)
            //{
            //    countryModel.Cities.Remove(city);
            //}
            ////Add
            //var addCities = country.Cities
            //    .Where(a => a.Id == null).ToList();
            //var addCitiesModel = mapper.Map<List<City>()
            //foreach (var city in addCities)
            //{
            //    countryModel.Cities.Add(city);
            //}

            return Ok();
        }

        private string CreateCountryUri(PaginationBase paginationBase,
            PaginationResourceUriType type)
        {
            switch (type)
            {
                case PaginationResourceUriType.PreviousPage:
                    var pre = new 
                    {
                        pageIndex = --paginationBase.PageIndex,
                        pageSize = paginationBase.PageSize,
                        orderBy = paginationBase.OrderBy,
                    };
                    return urlHelper.Link("GetCountries", pre);
                case PaginationResourceUriType.NextPage:
                    var next = new
                    {
                        pageIndex = ++paginationBase.PageIndex,
                        pageSize = paginationBase.PageSize,
                        orderBy = paginationBase.OrderBy,
                    };
                    return urlHelper.Link("GetCountries", next);
                case PaginationResourceUriType.CurrentPage:
                    
                default:
                    var current = new PaginationBase()
                    {
                        PageIndex = paginationBase.PageIndex,
                        PageSize = paginationBase.PageSize,
                        OrderBy = paginationBase.OrderBy,
                    };
                    return urlHelper.Link(nameof(Get), current);
            }
        }

    }
}
