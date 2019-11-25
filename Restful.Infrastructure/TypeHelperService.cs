using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Restful.Infrastructure
{
    public class TypeHelperService : ITypeHelperService
    {
        public bool TypeHasProperties<T>(string fields)
        {
            if (string.IsNullOrEmpty(fields))
            {
                return true;
            }
            var fieldAfterSplit = fields.Split(',');
            foreach (var field in fieldAfterSplit)
            {
                var propertyName = field.Trim();
                var propertyInfo = typeof(T).GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if(propertyInfo == null)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
