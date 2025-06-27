using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VillaWebApi.Models;

namespace VillaWebApi.Data.Configurations;

public class VillaNumberModelConfigurations : IEntityTypeConfiguration<VillaNumber>
{
    public void Configure(EntityTypeBuilder<VillaNumber> builder)
    {
        builder.HasKey(pk => pk.VillaNo);
        builder.Property(pk => pk.VillaNo)
            .ValueGeneratedNever();

        builder.Property(sp => sp.SpetialDeatils)
            .HasMaxLength(500);
        
        builder.Property(d => d.CreatedDate)
            .HasDefaultValueSql("getdate()");
        
        builder.Property(d => d.UpdatedDate)
            .HasDefaultValueSql("getdate()");

        builder.Property(d => d.VillaId).IsRequired(false);
        
        builder.HasOne(vn => vn.Villa)
            .WithMany(v => v.VillaNumber).HasForeignKey(vn => vn.VillaId);
    }
}
