using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VillaModels.Models;

namespace AppRepository.Data.Configurations;

public class BookingModelConfigurations : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.HasKey(b => b.Id);
        
        builder.HasOne(o => o.Customer)
            .WithMany(m => m.Bookings)
            .HasForeignKey(fk => fk.CustomerId);
        
        builder.HasOne(o => o.VillaNumber)
            .WithMany(m => m.Bookings)
            .HasForeignKey(fk => fk.VillaNumberId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Property(s => s.Status)
            .HasConversion<string>();
    }
}
