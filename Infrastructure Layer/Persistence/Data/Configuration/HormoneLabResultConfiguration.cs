using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuration
{
    public class HormoneLabResultConfiguration : IEntityTypeConfiguration<HormoneLabResult>
    {
        public void Configure(EntityTypeBuilder<HormoneLabResult> builder)
        {
            builder.ToTable("hormone_lab_results");

            // Configure the relationship with User
            // Set IsRequired(false) to allow seeding data without existing UserIds
            builder.HasOne(h => h.User)
                   .WithMany(u => u.HormoneLabResults)
                   .HasForeignKey(h => h.UserId)
                   .IsRequired(false);

            // Avoid SQL 'timestamp' type to prevent explicit insert errors
            builder.Property(h => h.CreatedAt).HasColumnType("nvarchar(max)");
        }
    }
}