using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.OwnsMany(u => u.RefreshTokens, t =>
            {
                t.ToTable("UserRefreshTokens");

                t.WithOwner().HasForeignKey("UserId");

                t.Property(rt => rt.Token)
                    .IsRequired()
                    .HasMaxLength(200);

                t.Property(rt => rt.CreatedOn)
                    .IsRequired();

                t.Property(rt => rt.ExpiresOn)
                    .IsRequired();

                // إضافة Shadow Property كـ Primary Key للجدول ده
                t.Property<int>("Id");
                t.HasKey("Id");
            });
        }
    }
}