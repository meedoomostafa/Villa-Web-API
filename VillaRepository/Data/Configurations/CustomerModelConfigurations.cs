using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VillaModels.Models;

namespace VillaRepository.Data.Configurations;

public class ApplicationUserModelConfigurations : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.Property(appUser =>appUser.FullName)
            .IsRequired().HasMaxLength(60);
        
        builder.Property(appUser =>appUser.Address)
            .IsRequired().HasMaxLength(200);
    }
}
