using AutoMapper;
using Restful.Core.Models;
using Restful.Infrastructure.Resourses;
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

            CreateMap<Country, CountryAddViewModel>();
            CreateMap<CountryAddViewModel, Country>();

            CreateMap<Country, CountryUpdateViewModel>();
            CreateMap<CountryUpdateViewModel, Country>();

            CreateMap<City, CityResource>();
            CreateMap<CityResource, City>();

            CreateMap<City, CityAddViewModel>();
            CreateMap<CityAddViewModel, City>();

            CreateMap<City, CityUpdateViewModel>();
            CreateMap<CityUpdateViewModel, City>();
        }
    }
}
