using UriShortener.Data.Entities;
using UriShortener.Data.Model.Dto;

namespace UriShortener.Data.Mappings;

public static class DomainToDtoMapper
{
  public static UriResponseDto MapToUriResponseDto(this ShortenedURI shortened){
    UriResponseDto dto = new(){
      Key = shortened.Key,
      Target = shortened.Target,
      CreatedAt = shortened.CreatedAt,
      ValidFor = shortened.ValidFor
    };
    return dto;
  }
}