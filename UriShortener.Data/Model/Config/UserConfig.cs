using UriShortener.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace UriShortener.Data.Config;

public class UserConfig : IEntityTypeConfiguration<User>
{
  public void Configure(EntityTypeBuilder<User> builder)
  {
    builder.ToTable("Users");

    builder.Property(u => u.Username)
      .HasColumnType("NVARCHAR")
      .HasMaxLength(64)
      .IsRequired();

    builder.Property(u => u.Email)
      .HasColumnType("VARCHAR")
      .HasMaxLength(255)
      .IsRequired();

    builder.Property(u => u.Pwd)
      .HasColumnType("VARCHAR")
      .HasMaxLength(255)
      .IsRequired();

    builder.Property(u => u.JoinedAt)
      .HasColumnType("DATETIME2")
      .HasPrecision(0)
      .IsRequired();

    builder.Property(u => u.EmailVerificationToken)
      .HasColumnType("VARCHAR(MAX)");
    builder.Property(u => u.EmailVerificationTokenIssuedAt)
      .HasColumnType("DATETIME2")
      .HasPrecision(0);
    builder.Property(u => u.EmailVerificationTokenUsedAt)
      .HasColumnType("DATETIME2")
      .HasPrecision(0);
    builder.Property(u => u.EmailVerificationTokenExpiresAt)
      .HasColumnType("DATETIME2")
      .HasPrecision(0);

    builder.Property(u => u.PasswordResetToken)
      .HasColumnType("VARCHAR(MAX)");
    builder.Property(u => u.PasswordResetTokenIssuedAt)
      .HasColumnType("DATETIME2")
      .HasPrecision(0);
    builder.Property(u => u.PasswordResetTokenUsedAt)
      .HasColumnType("DATETIME2")
      .HasPrecision(0);
    builder.Property(u => u.PasswordResetTokenExpiresAt)
      .HasColumnType("DATETIME2")
      .HasPrecision(0);

    builder.HasKey(u => u.Id);
    builder.HasIndex(u => u.Username).IsUnique();
    builder.HasIndex(u => u.Email).IsUnique();
    builder.HasIndex(u => u.Pwd).IsUnique();
    builder.HasMany(u => u.Roles).WithMany(r => r.Users).UsingEntity<UsersRoles>();
  }
}