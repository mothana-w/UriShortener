using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;

namespace UriShortener.Services;

public class EMailService : IEMailService
{
  public async Task Send(EMailDto dto){
    MimeMessage email = new();
    email.From.Add(MailboxAddress.Parse(dto.From));
    email.To.Add(MailboxAddress.Parse(dto.To));
    email.Subject = dto.Subject;
    email.Body = new TextPart(TextFormat.Html) { Text = dto.HTMLBody };

    var client = new SmtpClient();
    await client.ConnectAsync(dto.Host, dto.Port);
    await client.AuthenticateAsync(dto.From,dto.Password);
    await client.SendAsync(email);
    await client.DisconnectAsync(true);
  }
}