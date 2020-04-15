using System;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace DevelopIn.Cloud.CQRS.Infrastructure.RabbitMq
{
    public static class CqrsExtensions
    {
        public static IServiceCollection AddRabbitMqCqrs(
            this IServiceCollection services,
            RabbitMqSettings settings,
            params Type[] consumerTypes) =>
            services.AddMassTransit(x =>
            {
                x.AddConsumers(consumerTypes);

                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host(settings.HostAddress, host =>
                    {
                        host.Heartbeat(settings.Heartbeat);
                        host.Username(settings.Username);
                        host.Password(settings.Password);
                    });

                    foreach (var consumerType in consumerTypes)
                    {
                        cfg.ReceiveEndpoint(consumerType.FullName, ep =>
                        {
                            ep.PrefetchCount = 10;
                            ep.ConfigureConsumer(provider, consumerType);
                        });
                    }
                }));
            });

        public static IServiceCollection AddInMemoryCqrs(
            this IServiceCollection services,
            params Type[] consumerTypes) =>
            services.AddMassTransit(x =>
            {
                x.AddConsumers(consumerTypes);

                x.AddBus(provider => Bus.Factory.CreateUsingInMemory(cfg =>
                {
                    foreach (var consumerType in consumerTypes)
                    {
                        cfg.ReceiveEndpoint(consumerType.FullName, ep =>
                        {
                            ep.ConcurrencyLimit = 1;
                            ep.ConfigureConsumer(provider, consumerType);
                        });
                    }
                }));
            });

    }
}
