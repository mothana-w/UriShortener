using UriShortener.Data.Model.Dto;

namespace UriShortener.Services;

public class UriServiceResult
{
  public UriServiceStatus Status { get; set; }
  public UriResponseDto? Data { get; set; }
}
