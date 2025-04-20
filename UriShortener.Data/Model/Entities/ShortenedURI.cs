namespace UriShortener.Data.Entities;

public class ShortenedURI
{
  public string Key { get; set; } = null!; // PK
  public string Target { get; set; } = null!;
  public DateTime CreatedAt { get; set; }
  public DateTime ValidFor { get; set; }
  public int? CreatorId { get; set; }

  public User? Creator { get; set; }
}
