using Restful.Core.Models;
using Restful.Infrastructure.Resourses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restful.Infrastructure
{
    public class CountryMappingProperty : 
        PropertyMapping<CountryResource, Country>
    {
        public CountryMappingProperty():base(new Dictionary<string, List<MappedProperty>>
            (StringComparer.OrdinalIgnoreCase) 
        {
            [nameof(CountryResource.Abbreviation)] = new List<MappedProperty>
            {
                new MappedProperty{Name = nameof(Country.Abbreviation),Revert = false},
            },
            [nameof(CountryResource.ChineseName)] = new List<MappedProperty>
            {
                new MappedProperty{Name = nameof(Country.ChineseName),Revert = false},
            },
            [nameof(CountryResource.EnglishName)] = new List<MappedProperty>
            {
                new MappedProperty{Name = nameof(Country.EnglishName),Revert = false},
            },
        })
        {}
    }
}
