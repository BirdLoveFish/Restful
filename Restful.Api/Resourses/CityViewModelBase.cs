using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Restful.Api.Resourses
{
    public abstract class CityViewModelBase
    {
        [Required(ErrorMessage ="name is must")]
        public virtual string Name { get; set; }
        [Required(ErrorMessage = "description is must")]
        public virtual string Description { get; set; }
    }
}
