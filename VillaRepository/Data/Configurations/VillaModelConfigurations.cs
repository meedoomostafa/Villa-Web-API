using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VillaModels.Models;

namespace VillaRepository.Data.Configurations;

public class VillaModelConfigurations : IEntityTypeConfiguration<Villa>
{
    public void Configure(EntityTypeBuilder<Villa> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("getdate()");
        builder.Property(x => x.UpdatedAt)
            .HasDefaultValueSql("getdate()");

        builder.HasMany(vn => vn.VillaNumbers)
            .WithOne(v => v.Villa).HasForeignKey(v => v.VillaId);
    }
}
