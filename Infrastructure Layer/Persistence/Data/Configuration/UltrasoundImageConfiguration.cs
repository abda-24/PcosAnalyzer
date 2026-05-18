using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuration
{
    public class UltrasoundImageConfiguration : IEntityTypeConfiguration<UltrasoundImage>
    {
        public void Configure(EntityTypeBuilder<UltrasoundImage> builder)
        {
            builder.ToTable("ultrasound_images");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.ImagePath)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(i => i.UploadedAt)
                   .IsRequired()
                   .HasColumnType("datetime2");

            builder.Property(i => i.AiPrediction)
                   .HasMaxLength(50);

            builder.Property(i => i.Confidence);

            builder.Property(i => i.HeatmapBase64);

            builder.Property(i => i.UserId)
          .IsRequired()
          .HasMaxLength(450);
        }
    }
}