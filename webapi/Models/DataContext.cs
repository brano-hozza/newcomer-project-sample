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
    #region Required
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
        .HasOne(u => u.Position)
        .WithMany(b => b.Users)
        .OnDelete(DeleteBehavior.Cascade);
    }
    #endregion
}