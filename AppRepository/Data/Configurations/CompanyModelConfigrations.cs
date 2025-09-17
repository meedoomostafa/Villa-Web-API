using AppModels.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppRepository.Data.Configurations;

public class CompanyModelConfigrations : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.HasKey(c => c.ApplicationUserId);
        builder.HasOne(c => c.ApplicationUser)
            .WithOne()
            .HasForeignKey<Company>(c => c.ApplicationUserId);
        
        builder.Property(c => c.Country).HasMaxLength(50);
        builder.Property(c => c.City).HasMaxLength(50);
    }
}