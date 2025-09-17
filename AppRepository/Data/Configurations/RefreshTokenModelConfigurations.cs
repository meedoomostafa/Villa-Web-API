using AppModels.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppRepository.Data.Configurations;

public class RefreshTokenModelConfigurations : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(k => k.Id);
        
        builder.HasOne(p => p.User)
            .WithMany(r => r.RefreshTokens).HasForeignKey(p => p.UserId);
        
        builder.Property(p => p.UserId).IsRequired();
        builder.Property(p => p.TokenHash).IsRequired();
        
        builder.HasIndex(t => new { t.UserId, t.DeviceId }).IsUnique();
    }
}