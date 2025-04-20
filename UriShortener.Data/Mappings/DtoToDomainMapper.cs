using Microsoft.Identity.Client;
using UriShortener.Data.Entities;
using UriShortener.Data.Model.Dto;

namespace UriShortener.Data.Mappings;

public static class DtoToDomainMapper
{
  public static User MapToUser(this RegistrationRequestDto dto){
    User user = new(){
      Username = dto.Username,
      Email = dto.Email,
      Pwd = dto.Password,
      JoinedAt = DateTime.UtcNow,
    };
    return user;
  }
  public static ShortenedURI MapToShortenedUri(this UriWithKeyRequestDto dto){
    ShortenedURI shortend = new(){
      Target = dto.Target,
      CreatedAt = DateTime.UtcNow,
      ValidFor = DateTime.UtcNow.AddMinutes(dto.Minutes)
    };
    return shortend;
  }
  public static ShortenedURI MapToShortenedUri(this UriWithoutKeyRequestDto dto){
    ShortenedURI shortend = new(){
      Target = dto.Target,
      CreatedAt = DateTime.UtcNow,
    };
    return shortend;
  }
}