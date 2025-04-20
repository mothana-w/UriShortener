namespace UriShortener.Data.Model.Dto;

public class UriResponseDto
{
  public string Key { get; set; } = null!;
  public string Target { get; set; } = null!;
  public DateTime CreatedAt { get; set; }
  public DateTime ValidFor { get; set; }
}