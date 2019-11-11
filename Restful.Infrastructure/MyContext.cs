﻿using Microsoft.EntityFrameworkCore;
using Restful.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restful.Infrastructure
{
    public class MyContext:DbContext
    {
        public MyContext(DbContextOptions<MyContext> options)
        : base(options)
        { }

        public DbSet<Country> Countries { get; set; }
    }
}