using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VillaModels.Models;

namespace VillaRepository.Data.Configurations;

public class BookingModelConfigurations : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.HasKey(b => b.Id);
        
        builder.HasOne(o => o.ApplicationUser)
            .WithMany(m => m.Bookings)
            .HasForeignKey(fk => fk.ApplicationUserId);
        
        builder.HasOne(o => o.VillaNumber)
            .WithMany(m => m.Booking)
            .HasForeignKey(fk => fk.VillaNumberId);
        
        builder.Property(s => s.Status)
            .HasConversion<string>();
    }
}
