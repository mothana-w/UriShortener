namespace UriShortener.Data.Model.Dto;

public class RegistrationRequestDto
{
  public string Username { get; set; } = null!;
  public string Email { get; set; } = null!;
  public string Password { get; set; } = null!;
}