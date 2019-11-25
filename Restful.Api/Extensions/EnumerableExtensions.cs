using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Restful.Api.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<ExpandoObject> ToDynamicIEnumerable<TSource>
            (this IEnumerable<TSource> source,string fileds = null)
        {
            if(source == null)
            {
                throw new ArgumentNullException();
            }

            var expandoObject = new List<ExpandoObject>();
            var propertyInfoList = new List<PropertyInfo>();
            if (string.IsNullOrEmpty(fileds))
            {
                var propertyInfos = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                propertyInfoList.AddRange(propertyInfos);
            }
            else
            {
                var fieldsAfterSplit = fileds.Split(',').ToList();
                foreach (var field in fieldsAfterSplit)
                {
                    var propertyName = field.Trim();
                    var propertyInfo = typeof(TSource)
                        .GetProperty(propertyName, BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance);
                    if(propertyInfo == null)
                    {
                        throw new Exception();
                    }
                    propertyInfoList.Add(propertyInfo);
                }
            }
            foreach (TSource sourceObject in source)
            {
                var dataSingleObject = new ExpandoObject();
                foreach (var propertyInfo in propertyInfoList)
                {
                    var propertyValue = propertyInfo.GetValue(sourceObject);
                    ((IDictionary<string, object>)dataSingleObject).Add(propertyInfo.Name, propertyValue);
                }
                expandoObject.Add(dataSingleObject);
            }
            return expandoObject;
        }
    }
}
