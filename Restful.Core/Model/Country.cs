using System;
using System.Collections.Generic;
using System.Text;

namespace Restful.Core.Model
{
    public class Country
    {
        public Guid Id {get;set;}
        public string ChineseName { get; set; }
        public string EnglishName { get; set; }
        public string Abbreviation { get; set; }
    }
}
