using AutoMapper;
using Restful.Api.Resourses;
using Restful.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restful.Api.Configurations
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Country, CountryResource>();
            CreateMap<CountryResource, Country>();

            CreateMap<Country, CountryResource>();
            CreateMap<CountryAddViewModel, Country>();

            CreateMap<City, CityResource>();
            CreateMap<CityResource, City>();

            CreateMap<City, CityAddViewModel>();
            CreateMap<CityAddViewModel, City>();

        }
    }
}
