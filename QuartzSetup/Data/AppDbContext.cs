using Microsoft.EntityFrameworkCore;
using QuartzSetup.Models;

namespace QuartzSetup.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    internal DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    { }
}
