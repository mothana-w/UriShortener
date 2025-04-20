using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UriShortener.Data.Core;
using UriShortener.Data.Entities;

namespace Forum.Data.Config;

public class RoleConfig : IEntityTypeConfiguration<Role>
{
  public void Configure(EntityTypeBuilder<Role> builder)
  {
    builder.ToTable("Roles");

    builder.HasKey(r => r.Id);

    builder.Property(r => r.Title)
      .HasColumnType("VARCHAR")
      .HasMaxLength(64)
      .IsRequired()
      .HasConversion(
        r => r.ToString(),
        ur => (UserRole)Enum.Parse(typeof(UserRole), ur)
      );
    
    builder.HasData(
      new Role { Id = 1, Title = UserRole.Admin },
      new Role { Id = 2, Title = UserRole.PremiumUser },
      new Role { Id = 3, Title = UserRole.User }
    );
  }
}