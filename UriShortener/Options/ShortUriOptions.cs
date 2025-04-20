using System.ComponentModel.DataAnnotations;

namespace UriShortener.Options;

public class ShortUriOptions
{
  [Required]
  [Range(0, 64, ErrorMessage = $"{nameof(MaxKeyLength)} must be between 0 - 64 (Generated key length included)" )] // field in database limited to 64
  public ushort MaxKeyLength { get; set; }

  [Required]
  [Range(0, 64, ErrorMessage = $"{nameof(GeneratedKeyLength)} must be between 0 - 64" )]
  public ushort GeneratedKeyLength { get; set; }

  [Required]
  [Range(0, ulong.MaxValue, ErrorMessage = $"{nameof(MaxLifeTimeInMinutes)} out of range" )]
  public ulong MaxLifeTimeInMinutes { get; set; }

  [Required]
  [Range(0, ulong.MaxValue, ErrorMessage = $"{nameof(DefaultLifeTimeInMinutes)} out of range" )]
  public ulong DefaultLifeTimeInMinutes { get; set; }

}