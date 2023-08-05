
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hagca.Email;
public static class ServiceCollectionExtensions
{

    public static IServiceCollection AddHagcaEmail(this IServiceCollection services, Action<EmailConfigurationOptions> emailConfiguration)
    {
        services.Configure(emailConfiguration);

        services.AddTransient<IEmailService, MailKitEmailService>();

        return services;
    }

    public static IServiceCollection AddHagcaEmail(this IServiceCollection services, IConfigurationSection emailConfigurationSection)
    {
        services.Configure<EmailConfigurationOptions>(emailConfigurationSection);

        services.AddTransient<IEmailService, MailKitEmailService>();

        return services;
    }

}
