using Restful.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restful.Core.Models
{
    public class Country:IEntity
    {
        public Guid Id {get;set;}
        public string ChineseName { get; set; }
        public string EnglishName { get; set; }
        public string Abbreviation { get; set; }
        public List<City> Cities { get; set; }
    }
}
