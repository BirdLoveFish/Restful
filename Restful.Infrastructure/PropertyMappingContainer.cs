using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Restful.Core.Services;

namespace Restful.Infrastructure
{
    public class PropertyMappingContainer : IPropertyMappingContainer
    {
        private readonly IList<IPropertyMapping> propertyMappings = new List<IPropertyMapping>();
        public void Register<T>() where T : IPropertyMapping, new()
        {
            propertyMappings.Add(new T());
        }

        public IPropertyMapping Resolve<TSource, TDestination>() where TDestination : IEntity
        {
            var matchingMapping = propertyMappings
                .OfType<PropertyMapping<TSource, TDestination>>().ToList();
            if(matchingMapping.Count == 1)
            {
                return matchingMapping.First();
            }
            throw new Exception("too much ");
        }

        public bool ValidMappingExistsFor<TSource, TDestination>(string fields) where TDestination : IEntity
        {
            var propertyMapping = Resolve<TSource, TDestination>();
            if (string.IsNullOrEmpty(fields))
            {
                return true;
            }
            var fieldAfterSplit = fields.Split(',');
            foreach (var field in fieldAfterSplit)
            {
                var trimmedField = field.Trim();
                var indexOffFirstSpace = trimmedField.IndexOf(" ", StringComparison.Ordinal);
                var propertyName = indexOffFirstSpace == -1 ?
                    trimmedField : trimmedField.Remove(indexOffFirstSpace);
                if (!propertyMapping.MappingDictionary.ContainsKey(propertyName))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
