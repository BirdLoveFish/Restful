using Restful.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restful.Infrastructure
{
    public abstract class PropertyMapping<TSource,TDestination> : IPropertyMapping
    {
        public PropertyMapping(Dictionary<string, List<MappedProperty>> mappingDic)
        {
            MappingDictionary = mappingDic;

            MappingDictionary[nameof(IEntity.Id)] = new List<MappedProperty>
                {
                    new MappedProperty
                    {
                        Name = nameof(IEntity.Id),
                        Revert = false,
                    }
                };
        }
        public Dictionary<string, List<MappedProperty>> MappingDictionary { get; }
    }
}
