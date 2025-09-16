using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VillaModels.Models;

namespace AppRepository.Data.Configurations;

public class VillaModelConfigurations : IEntityTypeConfiguration<Villa>
{
    public void Configure(EntityTypeBuilder<Villa> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(v => v.LocationAddress)
            .HasMaxLength(500);

        builder.Property(v => v.Country)
            .HasMaxLength(100);

        builder.Property(v => v.City)
            .HasMaxLength(100);
        
        builder.Property(v => v.Latitude)
            .HasColumnType("decimal(9,6)");

        builder.Property(v => v.Longitude)
            .HasColumnType("decimal(9,6)");
        
        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("GETDATE()");
        builder.Property(x => x.UpdatedAt)
            .HasDefaultValueSql("GETDATE()");

        builder.HasMany(vn => vn.VillaNumbers)
            .WithOne(v => v.Villa)
            .HasForeignKey(v => v.VillaId);
        
        builder.HasOne(v => v.Company)
            .WithMany(v => v.Villas)
            .HasForeignKey(v => v.CompanyId);
    }
}
