using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restful.Api.Configurations
{
    public class CustomExceptionHandler: ExceptionHandlerOptions
    {
        public CustomExceptionHandler()
        {
            ExceptionHandler = async context => 
            {
                var handler =
                    context.Features.Get<IExceptionHandlerPathFeature>();
                await context.Response.WriteAsync
                ($"{{\"errMsg\":\"有错误,错误是{handler.Error.Message}," +
                $"错误的地址是{handler.Path}\"}}");
            };
        }
    }
}
