using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restful.Api.Extensions
{
    public class TestContainer
    {
        public readonly List<A> Containers = new List<A>();

        public void Register(A a)
        {
            Containers.Add(a);
        }

        public A Resolver(int i)
        {
            return Containers[i];
        }
    }

    public interface A
    {
        public string Name { get; set; }
    }

    public class B : A
    {
        public string Name { get; set; }
    }

    public class C : A
    {
        public string Name { get; set; }
    }
}
