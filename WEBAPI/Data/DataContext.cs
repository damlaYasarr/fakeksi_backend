using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WEBAPI.Models;

namespace WEBAPI.Data
{
    // pgadmin e migrate edelim. sistem çalışır haldeyken nginx ekleyelim. 
    // aws ile free rds nasıl oluşur bunu araştıralım. s3 -router- load balancing- 
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            //optionsBuilder.UseNpgsql("Server=.;Database=fakeksi;Trusted_Connection=true;TrustServerCertificate=true;");
            optionsBuilder.UseNpgsql("Host=localhost; Database=fakeksi; Username=postgres; Password=yourpassword; TrustServerCertificate=true;");

        }

        public DbSet<Users> Users { get; set; }
        public DbSet<FileUpload> Files { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Entry> Entries { get; set; }
        public DbSet<Followers> Followers { get; set; }
        public DbSet<Likes> Likes { get; set; }
        public DbSet<Msg> Msg { get; set; }
   
    }
}
