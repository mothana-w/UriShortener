using System.ComponentModel.DataAnnotations;

namespace UriShortener.Options;

public class EmailOptions
{
  [Required]
  public string Address { get; set; } = null!;

  [Required]
  public string Host { get; set; } = null!;

  [Required]
  public string Password { get; set; } = null!;

  [Required]
  [Range(0, int.MaxValue, ErrorMessage = $"{nameof(Port)} out of range" )]
  public int Port { get; set; }
}