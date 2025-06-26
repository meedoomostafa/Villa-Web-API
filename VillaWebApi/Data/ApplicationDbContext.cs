using Microsoft.EntityFrameworkCore;
using VillaWebApi.Data.Configrations;
using VillaWebApi.Models;
namespace VillaWebApi.Data;
public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Villa> Villas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        new VillaModelConfigrations().Configure(modelBuilder.Entity<Villa>());
    }
}
