using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuration
{
    public class ContactUsConfiguration : IEntityTypeConfiguration<ContactUsMessage>
    {
        public void Configure(EntityTypeBuilder<ContactUsMessage> builder)
        {
            builder.ToTable("contact_us_messages");

            builder.Property(m => m.Name).IsRequired().HasMaxLength(255);
            builder.Property(m => m.Email).IsRequired().HasMaxLength(255);
            builder.Property(m => m.Message).HasColumnType("text").IsRequired();

            // Fix 1: Make UserId optional to allow Seeding without a real User ID
            builder.HasOne(m => m.User)
                   .WithMany(u => u.ContactUsMessages)
                   .HasForeignKey(m => m.UserId)
                   .IsRequired(false);

            // Fix 2: Add SentAt configuration to avoid SQL timestamp issues
            builder.Property(m => m.SentAt).HasColumnType("nvarchar(max)");
        }
    }
}