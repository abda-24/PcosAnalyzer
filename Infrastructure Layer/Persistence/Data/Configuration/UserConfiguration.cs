using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("users");

            builder.Property(u => u.UserName).IsRequired().HasMaxLength(255);
            builder.Property(u => u.Email).HasMaxLength(255);
            builder.Property(u => u.FullName).IsRequired().HasMaxLength(255);
            builder.Property(u => u.CreatedAt).HasColumnType("datetime2");

            builder.OwnsMany(u => u.RefreshTokens, t =>
            {
                t.ToTable("UserRefreshTokens");
                t.WithOwner().HasForeignKey("UserId");
                t.Property<int>("Id");
                t.HasKey("Id");
            });

            builder.HasMany(u => u.ClinicalDataRecords)
                   .WithOne(c => c.User)
                   .HasForeignKey(c => c.UserId);

            builder.HasMany(u => u.HormoneLabResults)
                   .WithOne(h => h.User)
                   .HasForeignKey(h => h.UserId);

            builder.HasMany(u => u.UltrasoundImages)
                   .WithOne(i => i.User)
                   .HasForeignKey(i => i.UserId);

            builder.HasMany(u => u.ContactUsMessages)
                   .WithOne(m => m.User)
                   .HasForeignKey(m => m.UserId);
        }
    }
}
