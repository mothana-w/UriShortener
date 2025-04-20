namespace UriShortener.Services;

public class EMailDto
{
  public string Host { get; set; } = null!;
  public int Port { get; set; }
  public string Password { get; set; } = null!;
  public string From { get; set; } = null!;
  public string To { get; set; } = null!;
  public string Subject { get; set; } = string.Empty;
  public string HTMLBody { get; set; } = string.Empty;
}