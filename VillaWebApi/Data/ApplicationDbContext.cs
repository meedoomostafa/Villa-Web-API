using Microsoft.EntityFrameworkCore;
using VillaWebApi.Data.Configrations;
using VillaWebApi.Data.Configurations;
using VillaWebApi.Models;
namespace VillaWebApi.Data;
public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Villa> Villas { get; set; }
    public DbSet<VillaNumber> VillaNumbers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        new VillaModelConfigurations().Configure(modelBuilder.Entity<Villa>());
        new VillaNumberModelConfigurations().Configure(modelBuilder.Entity<VillaNumber>());
    }
}
