using System;
using System.Collections.Generic;
using System.Text;

namespace Restful.Infrastructure.Resourses
{
    public class LinkCollectionResourceWrapper<T>:LinkResourceBase
        where T : LinkResourceBase
    {
        public LinkCollectionResourceWrapper(IEnumerable<T> value)
        {
            Value = value;
        }
        public IEnumerable<T> Value { get; set; }
    }
}
