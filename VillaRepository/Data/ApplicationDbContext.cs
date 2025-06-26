using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VillaModels.Models;
using VillaRepository.Data.Configurations;
namespace VillaRepository.Data;
public sealed class ApplicationDbContext : IdentityDbContext<ApplicationUser,IdentityRole<int>, int>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Villa> Villas { get; set; }
    public DbSet<VillaNumber> VillaNumbers { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        new VillaModelConfigurations().Configure(modelBuilder.Entity<Villa>());
        new VillaNumberModelConfigurations().Configure(modelBuilder.Entity<VillaNumber>());
        new BookingModelConfigurations().Configure(modelBuilder.Entity<Booking>());
        new RefreshTokenModelConfigurations().Configure(modelBuilder.Entity<RefreshToken>());
    }
}
