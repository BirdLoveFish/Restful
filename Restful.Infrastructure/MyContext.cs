using Microsoft.EntityFrameworkCore;
using Restful.Core.Models;
using Restful.Infrastructure.ModelConfiguration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restful.Core
{
    public class MyContext:DbContext
    {
        public MyContext(DbContextOptions<MyContext> options)
        : base(options)
        { }

        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CountryConfiguration());
            modelBuilder.ApplyConfiguration(new CityConfiguration());
        }
    }
}
