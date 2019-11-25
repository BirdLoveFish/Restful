using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Restful.Api.Extensions
{
    public static class ObjectExtension
    {
        public static ExpandoObject ToDynamic<TSource>(this TSource source, string fields=null)
        {
            if (source == null)
            {
                throw new Exception();
            }

            var dataSingleObject = new ExpandoObject();
            if (string.IsNullOrEmpty(fields))
            {
                var propertyInfos = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (var propertyInfo in propertyInfos)
                {
                    var propertyValue = propertyInfo.GetValue(source);
                    ((IDictionary<string, object>)dataSingleObject).Add(propertyInfo.Name, propertyValue);
                }
                return dataSingleObject;
            }
            var fieldsAfterSplit = fields.Split(',').ToList();
            foreach (var field in fieldsAfterSplit)
            {
                var propertyName = field.Trim();
                var propertyInfo = typeof(TSource)
                    .GetProperty(propertyName, BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance);
                if (propertyInfo == null)
                {
                    throw new Exception();
                }
                var propertyValue = propertyInfo.GetValue(source);
                ((IDictionary<string, object>)dataSingleObject).Add(propertyInfo.Name, propertyValue);
            }
            return dataSingleObject;
        }
    }
}
