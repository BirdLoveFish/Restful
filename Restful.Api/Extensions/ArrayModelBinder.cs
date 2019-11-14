using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Restful.Infrastructure.Extensions
{
    class CusArrayModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            //只适用于集合类型
            if (!bindingContext.ModelMetadata.IsEnumerableType)
            {
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }
            //获取值
            string value = bindingContext.ValueProvider
                .GetValue(bindingContext.ModelName).FirstValue;

            //空就返回
            if (string.IsNullOrEmpty(value))
            {
                bindingContext.Result = ModelBindingResult.Success(null);
                return Task.CompletedTask;
            }
            //获取集合的成员类型
            //Type element = bindingContext.ModelType.GetTypeInfo().GenericTypeArguments[0];
            Type element = bindingContext.ModelMetadata.ElementType;
            //获取类型转换器
            TypeConverter convert = TypeDescriptor.GetConverter(element);

            //防止转换失败
            object[] array;
            try
            {
                array = value.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(a => convert.ConvertFromString(a))
                    .ToArray();
            }
            catch (Exception)
            {
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }
           
            //将object数组转为具体的类型数组
            var typeValue = Array.CreateInstance(element, array.Length);
            array.CopyTo(typeValue, 0);
            bindingContext.Model = typeValue;

            //返回绑定后的数据
            bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
            return Task.CompletedTask;
        }

    }

}
