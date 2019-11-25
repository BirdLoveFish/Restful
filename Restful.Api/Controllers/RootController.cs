using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Restful.Infrastructure.Resourses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restful.Api.Controllers
{
    [Route("api")]
    public class RootController:ControllerBase
    {
        private readonly IUrlHelper urlHelper;
        public RootController(IUrlHelper urlHelper)
        {
            this.urlHelper = urlHelper;
        }

        [HttpGet(Name = "GetRoot")]
        public IActionResult GetRoot([FromHeader(Name = "Accept")]string mediaType)
        {
            if (mediaType == "application/vnd.mycompany.heatoas+json")
            {
                var links = new List<LinkResource>()
                {
                    new LinkResource(urlHelper.Link("GetRoot",null),"self","GET"),
                    new LinkResource(urlHelper.Link("GetCountries",null),"self","GET"),
                    new LinkResource(urlHelper.Link("GetRoot",null),"self","GET"),
                };
                return Ok(links);
            }
            return NoContent();
        }


    }
}
