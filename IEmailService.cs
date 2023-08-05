namespace Hagca.Email;

public interface IEmailService
{
    public Task SendEmailAsync(Message message);
}
