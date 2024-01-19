using Microsoft.EntityFrameworkCore;
using FinalProject.Data.Entities;

namespace FinalProject.Back.Contexts
{
    public class CertificationDbContext:DbContext
    {
        public CertificationDbContext(DbContextOptions<CertificationDbContext> options):base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User { 
                    Id = 1, 
                    Email = "neilos@neko.com",
                    FirstName = "Neilos", 
                    LastName = "Kotsiopoulos", 
                    Phone = "123", 
                    Address = "Nea Smirni", 
                    Role =  "admin",
                    Password = @"AQAAAAIAAYagAAAAEPyPqDdsy1Dm0/9ha5foebLh3wvlwuycOtrqQVXdq66uW14eYgIKOaypZHfkANnKCQ==",
                    CreatedAt = DateTime.Now }
                ) ;
            modelBuilder.Entity<Certificate>().HasData(
                new Certificate { Id = 1, Title = "Microsoft Certified: Azure Fundamentals" },
                new Certificate { Id = 2, Title = "Microsoft Certified: Azure Administrator Associate" });
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
    }
}
