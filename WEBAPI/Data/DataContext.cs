﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WEBAPI.Models;

namespace WEBAPI.Data
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Users> Users { get; set; }
    }
}
