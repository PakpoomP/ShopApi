using InterviewTTBApi.Application.Interfaces;
using InterviewTTBApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InterviewTTBApi.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IApplicationDbContext
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<CartItem> CartItems => Set<CartItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(b =>
        {
            b.HasKey(p => p.Id);
            b.Property(p => p.Name).IsRequired().HasMaxLength(200);
            b.Property(p => p.Price).HasPrecision(18, 2);
        });

        modelBuilder.Entity<CartItem>(b =>
        {
            b.HasKey(c => c.Id);
            b.Property(c => c.CartId).IsRequired();
        });
    }
}
