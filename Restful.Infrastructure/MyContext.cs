using Microsoft.EntityFrameworkCore;
using Restful.Core.Models;
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
    }
}
