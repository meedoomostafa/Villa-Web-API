using AppModels.Models;
using AppRepository.Data.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppRepository.Data;
public sealed class ApplicationDbContext : IdentityDbContext<ApplicationUser,ApplicationRole, int>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Villa> Villas { get; set; }
    public DbSet<VillaNumber> VillaNumbers { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        new VillaModelConfigurations().Configure(modelBuilder.Entity<Villa>());
        new VillaNumberModelConfigurations().Configure(modelBuilder.Entity<VillaNumber>());
        new BookingModelConfigurations().Configure(modelBuilder.Entity<Booking>());
        new RefreshTokenModelConfigurations().Configure(modelBuilder.Entity<RefreshToken>());
        new CompanyModelConfigrations().Configure(modelBuilder.Entity<Company>());
        new CustomerModelConfigurations().Configure(modelBuilder.Entity<Customer>());
        new ReviewModelConfigurations().Configure(modelBuilder.Entity<Review>());
    }
}
