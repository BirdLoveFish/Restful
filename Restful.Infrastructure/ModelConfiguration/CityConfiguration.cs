using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restful.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restful.Infrastructure.ModelConfiguration
{
    class CityConfiguration: IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.Property(s => s.Name).IsRequired().HasMaxLength(10);
            builder.Property(s => s.Description).IsRequired().HasMaxLength(20);
        }
    }
}
