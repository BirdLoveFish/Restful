using Restful.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Restful.Api.Resourses
{
    public abstract class CountryViewModelBase
    {
        public virtual string ChineseName { get; set; }
        public virtual string EnglishName { get; set; }
        public virtual string Abbreviation { get; set; }
        public virtual List<CityUpdateViewModel> Cities { get; set; }
    }
}
