namespace Hagca.Email;
public class EmailConfigurationOptions
{
    public const string EmailConfiguration = nameof(EmailConfiguration);
    public required string SmtpServer { get; set; }
    public required int Port { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public required string FromName { get; set; }
    public required string FromAddress { get; set; }

}