using Microsoft.EntityFrameworkCore;
using VillaModels.Models;
using VillaRepository.Data.Configurations;
namespace VillaRepository.Data;
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
