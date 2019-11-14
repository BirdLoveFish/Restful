using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restful.Api.Extensions
{
    public class UnprocessableEntityObjectResultCus : ObjectResult
    {
        public UnprocessableEntityObjectResultCus(ModelStateDictionary modelState)
            :base(new SerializableError(modelState))
        {
            if(modelState == null)
            {
                throw new ArgumentNullException(nameof(modelState));
            }
            StatusCode = StatusCodes.Status422UnprocessableEntity; 
        }
    }
}
