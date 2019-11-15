using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restful.Infrastructure.Resourses
{
    public class CountryResource
    {
        public Guid Id { get; set; }
        public string ChineseName { get; set; }
        public string EnglishName { get; set; }
        public string Abbreviation { get; set; }
    }
}
