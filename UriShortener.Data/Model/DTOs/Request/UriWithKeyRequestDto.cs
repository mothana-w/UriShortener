namespace UriShortener.Data.Model.Dto;

public class UriWithKeyRequestDto
{
  public string Key { get; set; } = string.Empty;
  public string Target { get; set; } = string.Empty;
  public ulong Minutes { get; set; }
}