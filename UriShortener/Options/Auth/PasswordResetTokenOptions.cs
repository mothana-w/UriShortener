using System.ComponentModel.DataAnnotations;

namespace UriShortener.Options.Auth;

public class PasswordResetTokenOptions
{
  [Required]
  [Range(0, ushort.MaxValue, ErrorMessage = $"{nameof(Length)} out of range" )]
  public ushort Length { get; set; }

  [Required]
  [Range(0, ushort.MaxValue, ErrorMessage = $"{nameof(LifeTimeInMinutes)} out of range" )]
  public ushort LifeTimeInMinutes { get; set; }
}