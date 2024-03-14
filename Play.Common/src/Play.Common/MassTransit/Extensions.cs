using Microsoft.Extensions.DependencyInjection;
using Play.Common.Settings;
using MassTransit;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Play.Common.MassTransit
{
    public static class Extensions
    {
        public static IServiceCollection AddMassTransitWithRabbitMq(this IServiceCollection services)
        {
            services.AddMassTransit(busConfigurator =>
            {
                busConfigurator.SetKebabCaseEndpointNameFormatter();
                busConfigurator.AddConsumers(Assembly.GetEntryAssembly());
                busConfigurator.UsingRabbitMq((context, configurator) =>
                {
                    var rabbitMQSettings = context.GetRequiredService<IConfiguration>().GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();
                    configurator.Host(rabbitMQSettings.Host);
                    configurator.ConfigureEndpoints(context);
                    configurator.UseMessageRetry(retryConfigurator =>
                    {
                        retryConfigurator.Interval(3, TimeSpan.FromSeconds(5));
                    });
                });
            });

            return services;
        }
    }
}
