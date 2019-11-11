using System;
using System.Collections.Generic;
using System.Text;

namespace Restful.Core.Models
{
    public class City
    {
        public Guid Id { get; set; }
        public Guid CountryId { get; set; }
        public string Name { get; set; }
        public Country Country { get; set; }
    }
}
