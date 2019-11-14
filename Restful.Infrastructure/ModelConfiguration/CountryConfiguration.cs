using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restful.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restful.Infrastructure.ModelConfiguration
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.Property(s => s.ChineseName).IsRequired().HasMaxLength(10);
            builder.Property(s => s.EnglishName).IsRequired().HasMaxLength(5);
        }
    }
}
