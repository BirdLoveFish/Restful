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
using Microsoft.AspNetCore.Authorization;

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
        [Authorize]
        [HttpGet(Name ="GetCountries")]
        public async Task<IActionResult> Get(CountryResourceParameters parameters,
            [FromHeader(Name = "Accept")]string mediaType)
        {
            var countries = await countryRepository.GetCountriesAsync(parameters);
            var countryResource = mapper.Map<List<CountryResource>>(countries);

            if(mediaType == "application/vnd.mycompany.heatoas+json")
            {
                var meta = new
                {
                    countries.TotalItemsCount,
                    countries.PaginationBase.PageIndex,
                    countries.PaginationBase.PageSize,
                    countries.PageCount,
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(meta));

                var links = CreateCountriesLinks(parameters, countries.HasPrevious, countries.HasNext);
                var shapedResource = countryResource.ToDynamicIEnumerable(parameters.Fields);
                var shapedResourceWithLinks = shapedResource.Select(country =>
                {
                    var countryDit = country as IDictionary<string, object>;
                    var countryLinks = CreateCountryLinks((Guid)countryDit["Id"], parameters.Fields);
                    countryDit.Add("links", countryLinks);
                    return countryDit;
                });
                var linkedCountries = new
                {
                    value = shapedResourceWithLinks,
                    links,
                };
                return Ok(linkedCountries);
            }
            else
            {
                var perviousPagelink = countries.HasPrevious ?
                    CreateCountryUri(parameters, PaginationResourceUriType.PreviousPage):null;

                var nextPagelink = countries.HasNext ?
                    CreateCountryUri(parameters, PaginationResourceUriType.NextPage) : null;

                var meta = new
                {
                    countries.TotalItemsCount,
                    countries.PaginationBase.PageIndex,
                    countries.PaginationBase.PageSize,
                    countries.PageCount,
                    perviousPagelink,
                    nextPagelink
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(meta));
                return Ok(countryResource.ToDynamicIEnumerable(parameters.Fields));
            }
            
        }

        [HttpGet("{id}",Name = "GetCountry")]
        public async Task<IActionResult> GetCountry(Guid id,string fields)
        {
            var country = await countryRepository.GetCountryById(id);
            if(country == null)
            {
                return NotFound();
            }

            var countryResource = mapper.Map<CountryResource>(country);

            var links = CreateCountryLinks(id, fields);
            var result = countryResource.ToDynamic(fields) as IDictionary<string, object>;
            result.Add("links", links);
            return Ok(result);
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

        [HttpPost(Name = "AddCountry")]
        [RequestHeaderMatchingMediaType("Content-Type",
            new[] { "application/vnd.mycompany.country.create+json" })]
        [RequestHeaderMatchingMediaType("Content-Type",
            new[] { "application/vnd.mycompany.country.display+json" })]
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
            var links = CreateCountryLinks(countryModel.Id);
            var result = countryResource.ToDynamic() as IDictionary<string, object>;
            result.Add("links", links);

            return CreatedAtAction(nameof(GetCountry),new { id = countryResource.Id}, result);
        }

        [HttpPost(Name = "AddCountry")]
        [RequestHeaderMatchingMediaType("Content-Type",
            new[] { "application/vnd.mycompany.country.create2+json" })]

        public async Task<IActionResult> PostNew([FromBody]CountryAddViewModel country)
        {
            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResultCus(ModelState);
            }

            var countryModel = mapper.Map<Country>(country);

            await countryRepository.AddCountryAsync(countryModel);
            if (!await unitOfWork.SaveAsync())
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error when Add");
            }

            var countryResource = mapper.Map<CountryResource>(countryModel);
            var links = CreateCountryLinks(countryModel.Id);
            var result = countryResource.ToDynamic() as IDictionary<string, object>;
            result.Add("links", links);

            return CreatedAtAction(nameof(GetCountry), new { id = countryResource.Id }, result);
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

        [HttpDelete("{id}",Name = "DeleteCountry")]
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
                        fields = paginationBase.Fields
                    };
                    return urlHelper.Link("GetCountries", pre);
                case PaginationResourceUriType.NextPage:
                    var next = new
                    {
                        pageIndex = ++paginationBase.PageIndex,
                        pageSize = paginationBase.PageSize,
                        orderBy = paginationBase.OrderBy,
                        fields = paginationBase.Fields
                    };
                    return urlHelper.Link("GetCountries", next);
                case PaginationResourceUriType.CurrentPage:
                    
                default:
                    var current = new
                    {
                        pageIndex = paginationBase.PageIndex,
                        pageSize = paginationBase.PageSize,
                        orderBy = paginationBase.OrderBy,
                        fields = paginationBase.Fields
                    };
                    return urlHelper.Link("GetCountries", current);
            }
        }

        private IEnumerable<LinkResource> CreateCountryLinks(Guid id,string fields = null)
        {
            var links = new List<LinkResource>();
            if (string.IsNullOrEmpty(fields))
            {
                links.Add(new LinkResource(
                urlHelper.Link("GetCountry", new { id }),
                "self", "GET"));
            }
            else
            {
                links.Add(new LinkResource(
                urlHelper.Link("GetCountry", new { id , fields }),
                "self", "GET"));
            }

            links.Add(new LinkResource(
                urlHelper.Link("DeleteCountry", new { id }),
                "delete_country", "DELETE"));

            links.Add(new LinkResource(
                urlHelper.Link("GetCitiesForCountry", new {countryId =  id }),
                "get_cities", "GET"));

            return links;
        }

        private IEnumerable<LinkResource> CreateCountriesLinks(CountryResourceParameters parameters,
            bool hasPrevious, bool hasNext)
        {
            var links = new List<LinkResource>()
            {
                new LinkResource(
                    CreateCountryUri(parameters,PaginationResourceUriType.CurrentPage),
                    "self","GET"),
            };
            if (hasPrevious)
            {
                links.Add(
                    new LinkResource(CreateCountryUri(parameters, PaginationResourceUriType.PreviousPage),
                    "previous", "GET"));
            }
            if (hasNext)
            {
                links.Add(
                    new LinkResource(CreateCountryUri(parameters, PaginationResourceUriType.NextPage),
                    "next", "GET"));
            }
            return links;
        }

    }
}
