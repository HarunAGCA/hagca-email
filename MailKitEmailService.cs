using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;

namespace Hagca.Email;

public sealed class MailKitEmailService : IEmailService
{
    private readonly EmailConfigurationOptions _emailOptions;

    public MailKitEmailService(IOptions<EmailConfigurationOptions> emailOptions)
    {
        ArgumentNullException.ThrowIfNull(nameof(emailOptions));

        _emailOptions = emailOptions.Value;
    }

    public async Task SendEmailAsync(Message message)
    {
        using var client = new SmtpClient();

        try
        {
            client.Connect(_emailOptions.SmtpServer, _emailOptions.Port, true);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            client.Authenticate(_emailOptions.UserName, _emailOptions.Password);

            await client.SendAsync(CreateEmailMessage(message));
        }
        catch
        {
            //log an error message or throw an exception or both.
            throw;
        }
        finally
        {
            await client.DisconnectAsync(true);
            client.Dispose();
        }
    }

    private MimeMessage CreateEmailMessage(Message message)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(_emailOptions.FromName, _emailOptions.FromAddress));
        emailMessage.To.AddRange(message.To);
        emailMessage.Subject = message.Subject;

        var bodyBuilder = new BodyBuilder { HtmlBody = message.Content };

        if (message.Attachments != null && message.Attachments.Any())
        {
            byte[] fileBytes;
            foreach (var attachment in message.Attachments)
            {
                using (var ms = new MemoryStream())
                {
                    attachment.CopyTo(ms);
                    fileBytes = ms.ToArray();
                }

                bodyBuilder.Attachments.Add(attachment.FileName, fileBytes, ContentType.Parse(attachment.ContentType));
            }
        }

        emailMessage.Body = bodyBuilder.ToMessageBody();
        return emailMessage;
    }

}