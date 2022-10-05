namespace WebApi.Models;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options)
            : base(options)
    {
    }
    public DbSet<Position> Positions { get; set; } = null!;

    public DbSet<User> Users { get; set; } = null!;

    public DbSet<PositionChange> PositionChanges { get; set; } = null!;
    #region Required
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PositionChange>()
        .HasOne(u => u.User)
        .WithMany(b => b.PositionChanges)
        .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<PositionChange>()
        .HasOne(u => u.Position)
        .WithMany(b => b.PositionChanges)
        .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<User>()
        .HasOne(u => u.Position)
        .WithMany(b => b.Users)
        .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<User>()
        .HasMany(u => u.PositionChanges)
        .WithOne(b => b.User)
        .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Position>()
        .HasMany(u => u.Users)
        .WithOne(b => b.Position)
        .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Position>()
        .HasMany(u => u.PositionChanges)
        .WithOne(pc => pc.Position)
        .OnDelete(DeleteBehavior.Restrict);
    }
    #endregion
}