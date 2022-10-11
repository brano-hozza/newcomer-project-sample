namespace WebApi.Models;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;
/**
* Class for setting up database context with models
*/
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
        // Setting up relationships and deleting policy

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
        .OnDelete(DeleteBehavior.SetNull);

        // Data seeding

        modelBuilder.Entity<Position>().HasData(
            new Position { Id = 1, Name = "Tester" },
            new Position { Id = 2, Name = "Programátor" },
            new Position { Id = 3, Name = "Support" },
            new Position { Id = 4, Name = "Analytik" },
            new Position { Id = 5, Name = "Obchodník" },
            new Position { Id = 6, Name = "Iné" }
        );
    }
    #endregion
}