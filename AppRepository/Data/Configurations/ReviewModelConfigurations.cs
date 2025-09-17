using AppModels.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppRepository.Data.Configurations;

public class ReviewModelConfigurations : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Rating)
            .HasDefaultValue((byte)1);
        
        builder.Property(r => r.Comment)
            .HasMaxLength(300);

        builder.HasOne(r => r.Customer)
            .WithMany(r => r.Reviews)
            .HasForeignKey(r => r.CustomerId);
        
        builder.HasOne(r => r.Villa)
            .WithMany(r => r.Reviews)
            .HasForeignKey(r => r.VillaId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}