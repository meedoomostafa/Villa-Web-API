using AppModels.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AppModels.Models;


namespace AppRepository.Data.Configurations;

public class VillaNumberModelConfigurations : IEntityTypeConfiguration<VillaNumber>
{
    public void Configure(EntityTypeBuilder<VillaNumber> builder)
    {
        builder.HasKey(pk => pk.Id);
        builder.Property(pk => pk.Id)
            .ValueGeneratedNever();

        builder.Property(sp => sp.SpecialDetails)
            .HasMaxLength(500);
        
        builder.Property(d => d.CreatedAt)
            .HasDefaultValueSql("GETDATE()");
        
        builder.Property(d => d.UpdatedAt)
            .HasDefaultValueSql("GETDATE()");

        builder.Property(d => d.VillaId);
        
        builder.HasOne(vn => vn.Villa)
            .WithMany(v => v.VillaNumbers)
            .HasForeignKey(vn => vn.VillaId);
        
        builder.HasMany(vn => vn.Bookings)
            .WithOne(b => b.VillaNumber)
            .HasForeignKey(b => b.VillaNumberId);
    }
}
