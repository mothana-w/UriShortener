using System.ComponentModel.DataAnnotations;

namespace UriShortener.Options.Auth;

public class JwtOptions
{

  [Required]
  public string Issuer { get; set; } = null!;
  [Required]
  public string Audience { get; set; } = null!;

  [Required]
  [Range(0, int.MaxValue, ErrorMessage = $"{nameof(LifeTimeInMinutes)} out of range" )]
  public int LifeTimeInMinutes { get; set; }

  [Required]
  [Length(64, 64, ErrorMessage = $"{nameof(SigningKey)} length must be 64")]
  public string SigningKey { get; set; } = string.Empty;
}