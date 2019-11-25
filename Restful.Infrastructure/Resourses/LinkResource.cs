using System;
using System.Collections.Generic;
using System.Text;

namespace Restful.Infrastructure.Resourses
{
    public class LinkResource
    {
        public LinkResource(string href, string rel, string method)
        {
            Href = href;
            Rel = rel;
            Method = method;
        }
        //表示地址
        public string Href { get; set; }
        //表示动作的类型
        public string Rel { get; set; }
        //表示方法
        public string Method { get; set; }
    }
}
