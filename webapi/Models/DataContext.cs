namespace WebApi.Models;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options)
            : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
}