using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Dynamic.Core;
using System.Linq;

namespace Restful.Infrastructure
{
    public static class QueryableExtensions
    {
        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">待排序的query</param>
        /// <param name="orderBy">从客户端传过来的排序字符串</param>
        /// <param name="propertyMapping">映射表</param>
        /// <returns></returns>
        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source,
            string orderBy, IPropertyMapping propertyMapping)
        {
            if(source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var mappingDictionary = propertyMapping.MappingDictionary;
            if(mappingDictionary == null)
            {
                throw new ArgumentNullException(nameof(mappingDictionary));
            }

            if (string.IsNullOrEmpty(orderBy)){
                return source;
            }
            //将排序字符串分开
            var orderByAfterSplit = orderBy.Split(',');
            //遍历每一个排序属性
            foreach (var orderByClause in orderByAfterSplit.Reverse())
            {
                //把前后多余的空格去掉
                var trimmedOrderByCluse = orderByClause.Trim();
                //获得排序属性的最后是否以' desc'结尾
                var orderDescending = trimmedOrderByCluse.EndsWith(" desc");
                //找出第一个空格的位置
                var indexOffFirstSpace = trimmedOrderByCluse.IndexOf(" ", StringComparison.Ordinal);
                //拿到最终的属性，可以根据空格的位置获取，也可以根据是否以desc结尾获取
                var propertyName = indexOffFirstSpace == -1 ?
                    trimmedOrderByCluse : trimmedOrderByCluse.Remove(indexOffFirstSpace);
                //判读映射表中是否包含这个属性，不包含则抛出异常
                if (!mappingDictionary.ContainsKey(propertyName))
                {
                    throw new ArgumentNullException(nameof(propertyName));
                }
                //获得映射后的属性list
                var mappedProperties = mappingDictionary[propertyName];
                if(mappedProperties == null)
                {
                    throw new ArgumentNullException(nameof(mappedProperties));
                }
                //必须反转
                mappedProperties.Reverse();
                foreach (var destinationProperty in mappedProperties)
                {
                    if (destinationProperty.Revert)
                    {
                        orderDescending = !orderDescending;
                    }
                    //真正的排序方法
                    source = source.OrderBy(destinationProperty.Name + (orderDescending ? " desc" : " asc"), "collate nocase");
                }
            }
            return source;
        }
    }
}
