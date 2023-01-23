using Microsoft.EntityFrameworkCore;
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
            optionsBuilder.UseSqlServer("Server=.;Database=fakeksi;Trusted_Connection=true;TrustServerCertificate=true;");
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<FileUpload> Files { get; set; }
        public DbSet<Tag> Tags{ get; set; }
        public DbSet<Entry> Entry { get; set; }
        public DbSet<Followers> Followers { get; set; }

    }
}
