using Restful.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Restful.Infrastructure.Resourses
{
    public class CountryAddViewModel : CountryViewModelBase
    {
        public override string ChineseName { get; set; }
        //public string EnglishName { get; set; }
        //public string Abbreviation { get; set; }
        //public List<City> Cities { get; set; }
    }
}
