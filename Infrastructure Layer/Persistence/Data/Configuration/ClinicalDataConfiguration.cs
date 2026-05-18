using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuration
{
    public class ClinicalDataConfiguration : IEntityTypeConfiguration<ClinicalData>
    {
        public void Configure(EntityTypeBuilder<ClinicalData> builder)
        {
            builder.ToTable("clinical_data");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.UserId).IsRequired();

            builder.Property(c => c.BMI).HasColumnType("float");
            builder.Property(c => c.Weight).HasColumnType("float");
            builder.Property(c => c.Height).HasColumnType("float");

            builder.Property(c => c.Hb).HasColumnType("float");
            builder.Property(c => c.BetaHCG_I).HasColumnType("float");
            builder.Property(c => c.BetaHCG_II).HasColumnType("float");

            builder.Property(c => c.FSH).HasColumnType("float");
            builder.Property(c => c.LH).HasColumnType("float");
            builder.Property(c => c.FSH_LH).HasColumnType("float");

            builder.Property(c => c.Hip).HasColumnType("float");
            builder.Property(c => c.Waist).HasColumnType("float");
            builder.Property(c => c.WaistHipRatio).HasColumnType("float");

            builder.Property(c => c.TSH).HasColumnType("float");
            builder.Property(c => c.AMH).HasColumnType("float");
            builder.Property(c => c.PRL).HasColumnType("float");
            builder.Property(c => c.VitD3).HasColumnType("float");
            builder.Property(c => c.PRG).HasColumnType("float");

            builder.Property(c => c.RBS).HasColumnType("float");

            builder.Property(c => c.AvgFSizeL).HasColumnType("float");
            builder.Property(c => c.AvgFSizeR).HasColumnType("float");
            builder.Property(c => c.Endometrium).HasColumnType("float");

            builder.Property(c => c.CreatedAt).HasColumnType("datetime2");

            builder.HasOne(c => c.User)
                   .WithMany()
                   .HasForeignKey(c => c.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
