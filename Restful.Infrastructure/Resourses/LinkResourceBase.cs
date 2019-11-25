using System;
using System.Collections.Generic;
using System.Text;

namespace Restful.Infrastructure.Resourses
{
    public abstract class LinkResourceBase
    {
        public List<LinkResource> Links { get; set; } = new List<LinkResource>();
    }
}
