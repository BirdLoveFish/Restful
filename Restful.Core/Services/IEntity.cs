using System;
using System.Collections.Generic;
using System.Text;

namespace Restful.Core.Services
{
    public interface IEntity
    {
        public Guid Id { get; set; }
    }
}
