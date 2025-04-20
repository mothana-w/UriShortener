using UriShortener.Data.Model.Dto;

namespace UriShortener.Services;

public interface IUriService
{
  Task<UriServiceResult> GenerateUri(UriWithKeyRequestDto dto, int uid);
  Task<UriServiceResult> GenerateUri(UriWithoutKeyRequestDto dto);
  Task<string?> GetTarget(string key);
}