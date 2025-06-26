using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VillaModels.Models;

namespace VillaRepository.Data.Configurations;

public class RefreshTokenModelConfigurations : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(k => k.Id);
        
        builder.HasOne(p => p.User)
            .WithMany(r => r.RefreshTokens).HasForeignKey(p => p.UserId);
        
        builder.HasIndex(t => t.Token).IsUnique(false);
    }
}