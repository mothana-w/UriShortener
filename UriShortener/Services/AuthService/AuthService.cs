using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UriShortener.Data.Core;
using UriShortener.Data.Entities;
using UriShortener.Data.Mappings;
using UriShortener.Data.Model;
using UriShortener.Data.Model.Dto;
using UriShortener.Data.Repository;
using UriShortener.Options;
using UriShortener.Options.Auth;

namespace UriShortener.Services;

public class AuthService
( IOptions<JwtOptions> _jwtOpts
, IBaseRepository<User> _userRepo
, IOptions<EmailVerificationTokenOptions> _emailTokenOpts
, IEMailService _emailService
, IOptions<EmailOptions> _emailOpts
, AppDbContext _dbContext
) : IAuthService
{
  public async Task<RegistrationServiceStatus> RegisterAsync(RegistrationRequestDto dto, string baseUri, string verificationEndpoint, string verificationTokenQueryStrKey)
  {
    dto = dto.Trim();
    if (string.IsNullOrEmpty(dto.Username)) return RegistrationServiceStatus.EmptyUsername;
    if (string.IsNullOrEmpty(dto.Email)) return RegistrationServiceStatus.EmptyEmail;
    if (string.IsNullOrEmpty(dto.Password)) return RegistrationServiceStatus.EmptyPassword;

    if (!IsValidUsername(dto.Username)) return RegistrationServiceStatus.IncorrectUsernameFormat;

    dto.Email = dto.Email.ToLower();
    if (!IsValidEmail(dto.Email)) return RegistrationServiceStatus.IncorrectEmailFormat;

    if (await _userRepo.AnyAsync(u => u.Username == dto.Username || u.Email.ToLower() == dto.Email))
      return RegistrationServiceStatus.UserAlreadyExists;

    var pwdCheckRes = CheckPassword(dto.Password);
    if (pwdCheckRes is not null) return (RegistrationServiceStatus)pwdCheckRes;

    var user = dto.MapToUser();
    var HashedPwd = new PasswordHasher<User>().HashPassword(user, dto.Password);
    user.Pwd = HashedPwd;
    user.EmailVerificationToken = GenerateRandomToken();
    user.EmailVerificationTokenIssuedAt = DateTime.UtcNow;
    user.EmailVerificationTokenExpiresAt = DateTime.UtcNow.AddMinutes(_emailTokenOpts.Value.LifeTimeInMinutes);

    var role = new Role(UserRole.User);
    _dbContext.Entry(role).State = EntityState.Unchanged;
    user.Roles.Add(role);

    await SendEmailVerificationMessage
    (
      $"{baseUri}{verificationEndpoint}?{verificationTokenQueryStrKey}={user.EmailVerificationToken}"
      , user.Email
    );

    await _userRepo.AddAsync(user);
    return RegistrationServiceStatus.Success;
  }
  public async Task<LoginServiceResult> LoginAsync(LoginRequestDto dto)
  {
    dto = dto.Trim();
    if (string.IsNullOrEmpty(dto.UsernameOrEmail)) return new LoginServiceResult { Status = LoginServiceStatus.UsernameOrEmailEmpty };
    if (string.IsNullOrEmpty(dto.Password)) return new LoginServiceResult { Status = LoginServiceStatus.PasswordEmpty };
    
    #warning Find solution in repository pattern for include.
    // User? user = await _userRepo.GetSingleAsync(u => u.Username == dto.UsernameOrEmail || u.Email == dto.UsernameOrEmail.ToLower());
    User? user = await _dbContext.Users.Include(u => u.Roles).SingleOrDefaultAsync(u => u.Username == dto.UsernameOrEmail || u.Email == dto.UsernameOrEmail.ToLower());
    if
    (
      user is null
      || (new PasswordHasher<User>().VerifyHashedPassword(user, user.Pwd, dto.Password) == PasswordVerificationResult.Failed)
    )
    return new LoginServiceResult{ Status = LoginServiceStatus.UsernameEmailOrPasswordIncorrect };

    if (user.EmailVerificationTokenUsedAt is null)
    { return new LoginServiceResult{ Status = LoginServiceStatus.EmailNotVerified }; }

    string jwt = GenerateJwt(user);    
    return new LoginServiceResult{ Status = LoginServiceStatus.Success, Jwt = jwt };
  }
  public async Task<TokenVerificationStatus> VerifyAsync(string token){
    var user = await _userRepo.GetFirstOrDefaultAsync(u => u.EmailVerificationToken == token);
    if (user is null
    || user.EmailVerificationTokenUsedAt is not null)
      return TokenVerificationStatus.TokenInvalid;

    if (user.EmailVerificationTokenExpiresAt <= DateTime.UtcNow)
      return TokenVerificationStatus.TokenExpired;
    
    user.EmailVerificationTokenUsedAt = DateTime.UtcNow;
    await _userRepo.SaveChangesAsync();
    return TokenVerificationStatus.TokenValid;
  }
  private string GenerateJwt(User user)
  {
   string userRoles = string.Join(", ", user.Roles.Select(ur => ur.Title.ToString()));

    var tokenHandler = new JwtSecurityTokenHandler();
    var tokenDescriptor = new SecurityTokenDescriptor {
      Issuer = _jwtOpts.Value.Issuer,
      Audience = _jwtOpts.Value.Audience,
      SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOpts.Value.SigningKey)), SecurityAlgorithms.HmacSha512),

      Subject = new ClaimsIdentity(
        new List<Claim> {
          new (ClaimTypes.NameIdentifier, user.Id.ToString()),
          new (ClaimTypes.Name, user.Username),
          new (ClaimTypes.Email, user.Email),
          new (ClaimTypes.Role, userRoles)
        }
      )
    };

    var token = tokenHandler.CreateToken(tokenDescriptor);
    return tokenHandler.WriteToken(token);
  }
  private bool IsValidUsername(string username){
    if (username.Length > 64)
      return false;
    return true;
  }
  private bool IsValidEmail(string email)
  {
    if (email.Length > 255 || email.EndsWith("."))
      return false;

    try {
      var addr = new System.Net.Mail.MailAddress(email);
      return addr.Address == email;
    }
    catch {
      return false;
    }
  }
  private RegistrationServiceStatus? CheckPassword(string Password)
  {
    if (Password.Length < 12)
      return RegistrationServiceStatus.ShortPassword;
    if (Password.Length > 255)
      return RegistrationServiceStatus.LongPassword;
    return null;
  }
  private string GenerateRandomToken() => Convert.ToHexString(RandomNumberGenerator.GetBytes(_emailTokenOpts.Value.Length));
  private async Task SendEmailVerificationMessage(string verificationUrl, string to)
  {
    try {
      string htmlMsgBody = "<p>To complete registration process to <span style='color:blue'>http://urishortener.com</span> please click below button " +
                           "if you didn't try to sign-up please report at <span style='color:blue'>http://support.urishortener.com</span> to prevent any " +
                           "illegal activity commited with your account</p>" +
                           $"<button src='{verificationUrl}' style='padding:10px; background-color:yellow; font-weight:800;'> Verify </button>";
      EMailDto dto = new(){
        From = _emailOpts.Value.Address,
        To = to,
        Subject = "Uri Shortener Email Verification Message",
        HTMLBody = htmlMsgBody,
        Host = _emailOpts.Value.Host,
        Password = _emailOpts.Value.Password,
        Port = _emailOpts.Value.Port
      };
      await _emailService.Send(dto);
    }
    catch {
      throw new InvalidOperationException("Something went wrong sending verification code.");
    }
  }
}
