using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VillaModels.Models;

namespace VillaRepository.Data.Configurations;

public class ApplicationUserModelConfigurations : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(b => b.FirstName)
            .IsRequired().HasMaxLength(30);
        
        builder.Property(b => b.LastName)
            .HasMaxLength(30);
    }
}
