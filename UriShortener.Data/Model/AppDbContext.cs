using Microsoft.EntityFrameworkCore;
using UriShortener.Data.Config;
using UriShortener.Data.Entities;

namespace UriShortener.Data.Model;

public class AppDbContext : DbContext
{
  public DbSet<User> Users { get; set; }
  public DbSet<ShortenedURI> ShortenedURIs { get; set; }
  public AppDbContext(DbContextOptions<AppDbContext> dbContextOpts) : base(dbContextOpts) {}

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfig).Assembly);
  }
}