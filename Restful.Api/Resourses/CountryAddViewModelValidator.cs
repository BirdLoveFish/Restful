using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace Restful.Api.Resourses
{
    public class CountryAddViewModelValidator<T>:AbstractValidator<T> where T : CountryViewModelBase
    {
        public CountryAddViewModelValidator()
        {
            //RuleFor(c => c.ChineseName)
            //    .NotEmpty().WithName("中文").WithMessage("ChineseName is need")
            //    .MaximumLength(10).WithMessage("ChineseName is too long");

            //RuleFor(c => c.EnglishName)
            //    .NotEmpty().WithName("english").WithMessage("EnglishName is need")
            //    .MaximumLength(5).WithMessage("EnglishName is too long");
        }
    }
}
