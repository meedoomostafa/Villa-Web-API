using AppModels.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppRepository.Data.Configurations;

public class CustomerModelConfigurations : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.ApplicationUserId);
        builder.HasOne(c => c.ApplicationUser)
            .WithOne()
            .HasForeignKey<Customer>(c => c.ApplicationUserId);
        
        builder.Property(appUser =>appUser.FullName)
            .IsRequired().HasMaxLength(60);
        
        builder.Property(appUser =>appUser.Address)
            .IsRequired().HasMaxLength(200);
    }
}
