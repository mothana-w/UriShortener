using UriShortener.Data.Entities;
using UriShortener.Data.Model.Dto;
using UriShortener.Data.Repository;
using UriShortener.Data.Core;
using UriShortener.Data.Mappings;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using UriShortener.Options;

# warning validate target uri and scheme before inserting in database
namespace UriShortener.Services;

public class UriService(IBaseRepository<ShortenedURI> _uriRepo, IOptions<ShortUriOptions> _shUriOpts) : IUriService
{
  public async Task<UriServiceResult> GenerateUri(UriWithKeyRequestDto dto, int uid){
    dto = dto.Trim();

    if (string.IsNullOrEmpty(dto.Target))
      return new UriServiceResult { Status = UriServiceStatus.EmptyTarget };

    if (dto.Minutes <= 0 || dto.Minutes > _shUriOpts.Value.MaxLifeTimeInMinutes)
      return new UriServiceResult { Status = UriServiceStatus.InvalidTime };

    var shortened = dto.MapToShortenedUri();
    shortened.CreatorId = uid;

    var key = GenerateKey(_shUriOpts.Value.GeneratedKeyLength);
    if (!string.IsNullOrEmpty(dto.Key)){

      string cleaned = Regex.Replace(dto.Key, @"\s+", "-"); // replace spaces with hyphens
      cleaned = Regex.Replace(cleaned, @"[^a-zA-Z0-9\-]", ""); // keeps only [a-z][A-Z][0-9] and -

      int allowedKeyLength = _shUriOpts.Value.MaxKeyLength - _shUriOpts.Value.GeneratedKeyLength - 2; // 2 for ++ between key and cleand below
      if (cleaned.Length > allowedKeyLength)
        return new UriServiceResult { Status = UriServiceStatus.LongKey };

      shortened.Key = $"{key}--{cleaned}";
    }
    else
      shortened.Key = key;

    if (await _uriRepo.AnyAsync(shUri => shUri.Key.Equals(shortened.Key)))
      await GenerateUri(dto, uid);

    await _uriRepo.AddAsync(shortened);
    return new UriServiceResult { Status = UriServiceStatus.Success, Data = shortened.MapToUriResponseDto() };
  }

  public async Task<UriServiceResult> GenerateUri(UriWithoutKeyRequestDto dto){
    dto = dto.Trim();

    if (string.IsNullOrEmpty(dto.Target))
      return new UriServiceResult { Status = UriServiceStatus.EmptyTarget };

    var shortened = dto.MapToShortenedUri();

    var key = GenerateKey(_shUriOpts.Value.GeneratedKeyLength);
    shortened.Key = key;
    shortened.ValidFor = DateTime.UtcNow.AddMinutes(_shUriOpts.Value.DefaultLifeTimeInMinutes);
    

    if (await _uriRepo.AnyAsync(shUri => shUri.Key.Equals(shortened.Key)))
      await GenerateUri(dto);

    await _uriRepo.AddAsync(shortened);
    return new UriServiceResult { Status = UriServiceStatus.Success, Data = shortened.MapToUriResponseDto() };
  }

  public async Task<string?> GetTarget(string key){
    var result = await _uriRepo.GetFirstOrDefaultAsync(shUri => shUri.Key.Equals(key));

    if (result is null || result.ValidFor < DateTime.UtcNow)
      return null;

    return result.Target;
  }

  private static Random random = new Random();
  private static string GenerateKey(int length)
  {
    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
    var a = Enumerable.Repeat(chars, length);
    return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
  }
}