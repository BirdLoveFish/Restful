using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restful.Api.Extensions
{
    public static class TestContainerExtension
    {
        public static void AddTestContainer(this IServiceCollection services)
        {
            var con = new TestContainer();
            con.Register(new B { Name = "ccc"});
            con.Register(new C { Name = "ttt" });
            con.Register(new B { Name = "yyy" });
            con.Register(new C { Name = "uuu" });
            con.Register(new B { Name = "iii" });
            con.Register(new B { Name = "ooo" });
            services.AddSingleton<TestContainer>(con);
            
        }
    }
}
