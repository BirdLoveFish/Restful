using System;
using System.Collections.Generic;
using System.Text;

namespace Restful.Core.Models
{
    public class CountryResourceParameters:PaginationBase
    {
        public string ChineseName { get; set; }
        public string EnglishName { get; set; }
    }
}
