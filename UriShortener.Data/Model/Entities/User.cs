using Microsoft.Identity.Client;

namespace UriShortener.Data.Entities;

public class User
{
  public int Id { get; set; }
  public string Username { get; set; } = null!;
  public string Email { get; set; } = null!;
  public string Pwd { get; set; } = null!;
  public DateTime JoinedAt { get; set; }
  public string? EmailVerificationToken { get; set; }
  public DateTime? EmailVerificationTokenIssuedAt { get; set; }
  public DateTime? EmailVerificationTokenUsedAt { get; set; }
  public DateTime? EmailVerificationTokenExpiresAt { get; set; }
  public string? PasswordResetToken { get; set; }
  public DateTime? PasswordResetTokenIssuedAt { get; set; }
  public DateTime? PasswordResetTokenUsedAt { get; set; }
  public DateTime? PasswordResetTokenExpiresAt { get; set; }

  public ICollection<ShortenedURI> ShortenedURIs { get; set; } = new List<ShortenedURI>();
  public ICollection<Role> Roles { get; set; } = new List<Role>();
  public ICollection<UsersRoles> UsersRoles { get; set; } = new List<UsersRoles>();
}
