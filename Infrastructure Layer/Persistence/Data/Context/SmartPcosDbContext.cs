using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Data.Context
{
    public class SmartPcosDbContext : IdentityDbContext<ApplicationUser>
    {
        public SmartPcosDbContext(DbContextOptions<SmartPcosDbContext> options)
            : base(options)
        {
        }

        public DbSet<ClinicalData> ClinicalData { get; set; }
        public DbSet<HormoneLabResult> HormoneLabResults { get; set; }
        public DbSet<UltrasoundImage> UltrasoundImages { get; set; }
        public DbSet<ContactUsMessage> ContactUsMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Applies UserConfiguration, ClinicalDataConfiguration, etc.
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SmartPcosDbContext).Assembly);

            modelBuilder.Entity<ClinicalData>().ToTable("clinical_data");
            modelBuilder.Entity<UltrasoundImage>().ToTable("ultrasound_images");
            modelBuilder.Entity<HormoneLabResult>().ToTable("hormone_lab_results");
            modelBuilder.Entity<ContactUsMessage>().ToTable("contact_us_messages");

            modelBuilder.Entity<IdentityRole>().ToTable("roles");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("user_roles");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("user_claims");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("user_logins");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("user_tokens");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("role_claims");
        }
    }
}
