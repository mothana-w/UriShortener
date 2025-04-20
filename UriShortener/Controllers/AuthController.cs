using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using UriShortener.Data.Model.Dto;
using UriShortener.Services;

namespace UriShortener.Controllers;

[ApiController]
[Route("api/")]
public class AuthController(IAuthService _authService) : ControllerBase
{
  private string _emailVerificationEndpoint { get; } = "/api/verify";
  private string _verificationTokenQueryStrKey { get; } = "token";
  [HttpPost("register")]
  public async Task<IActionResult> RegisterAsync(RegistrationRequestDto dto){
    try {
      string baseUri = $"{Request.Scheme}://{Request.Host}";
      var result = await _authService.RegisterAsync(dto, baseUri, _emailVerificationEndpoint, _verificationTokenQueryStrKey);
      if (result.Equals(RegistrationServiceStatus.EmptyUsername)) return BadRequest("Username required");
      else if (result.Equals(RegistrationServiceStatus.EmptyEmail)) return BadRequest("Email required");
      else if (result.Equals(RegistrationServiceStatus.EmptyPassword)) return BadRequest("Password required");
      else if (result.Equals(RegistrationServiceStatus.IncorrectUsernameFormat)) return BadRequest("Username must only contain [[0 - 1] | [a - b] | [A - B] | [أ - ي]]");
      else if (result.Equals(RegistrationServiceStatus.IncorrectEmailFormat)) return BadRequest("Incorrect email format");
      else if (result.Equals(RegistrationServiceStatus.ShortPassword)) return BadRequest("Password length must at least consist of 12 degits");
      else if (result.Equals(RegistrationServiceStatus.LongPassword)) return BadRequest("Password can't be more than 255 degit");
      else if (result.Equals(RegistrationServiceStatus.WeakPassword)) return BadRequest("Password must consist have at least one of the following [[0 - 1] | [a - b] | [A - B] | [أ - ي] <symbols>]");
      else if (result.Equals(RegistrationServiceStatus.UserAlreadyExists)) return BadRequest("User with username or email already exists");
      else
        return Ok();
    }
    catch (InvalidOperationException ex) { return BadRequest(ex.Message); }
    catch { return BadRequest("Something went wrong processing data"); }
  }

  [HttpPost("login")]
  public async Task<IActionResult> LoginAsync(LoginRequestDto dto){
    var result = await _authService.LoginAsync(dto);

    if (result.Status.Equals(LoginServiceStatus.UsernameOrEmailEmpty)) return BadRequest("Username Or Email field required");
    else if (result.Status.Equals(LoginServiceStatus.PasswordEmpty)) return BadRequest("Password field required");
    else if (result.Status.Equals(LoginServiceStatus.UsernameEmailOrPasswordIncorrect)) return BadRequest("Invalid username, email or password");
    else if (result.Status.Equals(LoginServiceStatus.EmailNotVerified)) return BadRequest("Email verification required");
    else
      return Ok(result.Jwt);
  }

  [HttpPost("verify")]
  public async Task<IActionResult> VerifyAsync([FromQuery] string token){
    var result = await _authService.VerifyAsync(token);
    if (result.Equals(TokenVerificationStatus.TokenInvalid)) return BadRequest("Invalid Token");
    else if (result.Equals(TokenVerificationStatus.TokenExpired)) return BadRequest("Token Expired");
    else
      return Ok();
  }
}