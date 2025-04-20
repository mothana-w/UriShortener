using UriShortener.Data.Model.Dto;

namespace UriShortener.Services;

public interface IAuthService
{
  Task<RegistrationServiceStatus> RegisterAsync(RegistrationRequestDto dto, string baseUri, string verificationEndpoint, string verificationTokenQueryStrKey);
  Task<LoginServiceResult> LoginAsync(LoginRequestDto dto);
  Task<TokenVerificationStatus> VerifyAsync(string token);
}
