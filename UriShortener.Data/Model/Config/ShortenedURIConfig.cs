using UriShortener.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UriShortener.Data.Config;

public class ShortenedURIConfig : IEntityTypeConfiguration<ShortenedURI>
{
  public void Configure(EntityTypeBuilder<ShortenedURI> builder)
  {
    builder.ToTable("ShortenedURIs");

    builder.Property(shUri => shUri.Key)
      .HasColumnType("VARCHAR")
      .HasMaxLength(64)
      .IsRequired();

    builder.Property(shUri => shUri.CreatedAt)
      .HasColumnType("DATETIME2")
      .HasPrecision(0)
      .IsRequired();

    builder.Property(shUri => shUri.ValidFor)
      .HasColumnType("DATETIME2")
      .HasPrecision(0)
      .IsRequired();

    builder.Property(shUri => shUri.CreatorId)
      .HasColumnType("INT")
      .IsRequired(false);

    builder.HasKey(shUri => shUri.Key);
    builder.HasIndex(shUri => shUri.Key).IsUnique();

    builder
    .HasOne(shUri => shUri.Creator)
    .WithMany(u => u.ShortenedURIs)
    .HasForeignKey(shUri => shUri.CreatorId)
    .OnDelete(DeleteBehavior.Cascade);
  }
}