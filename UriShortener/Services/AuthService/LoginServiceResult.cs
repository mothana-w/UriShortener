using UriShortener.Services;

public class LoginServiceResult
{
  public LoginServiceStatus Status { get; set; }
  public string Jwt { get; set; } = string.Empty;
}

