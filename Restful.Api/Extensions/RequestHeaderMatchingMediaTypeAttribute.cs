using Microsoft.AspNetCore.Mvc.ActionConstraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restful.Api.Extensions
{
    [AttributeUsage(AttributeTargets.All,AllowMultiple = true)]
    public class RequestHeaderMatchingMediaTypeAttribute : Attribute, IActionConstraint
    {
        private readonly string requestHeader;
        private readonly string[] mediaTypes;
        public RequestHeaderMatchingMediaTypeAttribute(string requestHeader, string[] mediaTypes)
        {
            this.requestHeader = requestHeader;
            this.mediaTypes = mediaTypes;
        }
        public int Order => 0;

        public bool Accept(ActionConstraintContext context)
        {
            var requestHeaders = context.RouteContext.HttpContext.Request.Headers;
            if (!requestHeaders.ContainsKey(requestHeader))
            {
                return false;
            }
            foreach (var mediaType in mediaTypes)
            {
                var match = string.Equals(requestHeaders[requestHeader].ToString(),
                    mediaType, StringComparison.OrdinalIgnoreCase);
                if (match)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
