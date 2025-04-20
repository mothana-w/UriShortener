using Microsoft.EntityFrameworkCore.Metadata.Internal;
using UriShortener.Data.Model.Dto;

namespace UriShortener.Data.Core;

public static class DtoTrimmer{
  public static RegistrationRequestDto Trim(this RegistrationRequestDto dto){
    return new RegistrationRequestDto {
      Username = dto.Username.Trim(),
      Email = dto.Email.Trim(),
      Password = dto.Password.Trim()
    };
  }
  public static LoginRequestDto Trim(this LoginRequestDto dto){
    return new LoginRequestDto {
      UsernameOrEmail = dto.UsernameOrEmail.Trim(),
      Password = dto.Password.Trim()
    };
  }
  public static UriWithKeyRequestDto Trim(this UriWithKeyRequestDto dto){
    return new UriWithKeyRequestDto{
      Key = dto.Key.Trim(),
      Target = dto.Target.Trim(),
      Minutes = dto.Minutes
    };
  }
  public static UriWithoutKeyRequestDto Trim(this UriWithoutKeyRequestDto dto){
    return new UriWithoutKeyRequestDto{
      Target = dto.Target.Trim(),
    };
  }
}