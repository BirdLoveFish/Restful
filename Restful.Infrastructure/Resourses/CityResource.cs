using Restful.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restful.Infrastructure.Resourses
{
    public class CityResource:LinkResourceBase
    {
        public Guid Id { get; set; }
        public Guid CountryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
