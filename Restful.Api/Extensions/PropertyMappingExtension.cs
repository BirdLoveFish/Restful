using Microsoft.Extensions.DependencyInjection;
using Restful.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restful.Api.Extensions
{
    public static class PropertyMappingExtension
    {
        public static void AddPropertyMapping(this IServiceCollection services)
        {
            var propertyMappingContainer = new PropertyMappingContainer();
            propertyMappingContainer.Register<CountryMappingProperty>();
            services.AddSingleton<IPropertyMappingContainer>(propertyMappingContainer);
        }
    }
}
