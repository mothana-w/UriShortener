namespace UriShortener.Services;

public interface IEMailService
{
  Task Send(EMailDto dto);
}