using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restful.Api.Configurations
{
    public class CustomExceptionHandler: ExceptionHandlerOptions
    {
        private readonly ILoggerFactory loggerFactory;

        public CustomExceptionHandler(ILoggerFactory loggerFactory)
        {
            ExceptionHandler = async context => 
            {
                var logger = loggerFactory.CreateLogger("Globle Exception Logger");
                var handler =
                    context.Features.Get<IExceptionHandlerPathFeature>();
                if(handler != null)
                {
                    logger.LogError($"bbbbbbbb");
                    await context.Response.WriteAsync
                    ($"错误是{handler.Error.Message},错误的地址是{handler.Path}");
                }
                
            };
            this.loggerFactory = loggerFactory;
        }
    }
}
