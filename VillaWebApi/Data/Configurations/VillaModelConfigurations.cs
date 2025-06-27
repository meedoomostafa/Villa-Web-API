using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VillaWebApi.Data.Configrations;

public class VillaModelConfigurations : IEntityTypeConfiguration<Models.Villa>
{
    public void Configure(EntityTypeBuilder<Models.Villa> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CreatedAt).HasDefaultValueSql("getdate()");
        builder.Property(x => x.UpdatedAt).HasDefaultValueSql("getdate()");
    }
}
