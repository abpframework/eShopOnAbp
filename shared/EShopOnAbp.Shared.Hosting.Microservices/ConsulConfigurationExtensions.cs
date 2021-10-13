using System;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EShopOnAbp.Shared.Hosting.Microservices
{
    public static class ConsulConfigurationExtensions
    {
        public static IServiceCollection AddConsulConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConsulClient>(p => new ConsulClient(consulConfig =>
            {
                var address = configuration.GetValue<string>("Consul:Host");
                consulConfig.Address = new Uri(address);
            }));
            return services;
        }

        public static string UseConsul(this IApplicationBuilder app)
        {
            var appLifetime = app.ApplicationServices.GetService<IHostApplicationLifetime>();
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var configuration = scope.ServiceProvider.GetService<IConfiguration>();

                bool isEnabled = configuration.GetValue<bool>("Consul:Enabled");
                string serviceName = configuration.GetValue<string>("Consul:Service");
                var appString = configuration.GetValue<string>("App:SelfUrl");
                Uri appUrl = new Uri(appString, UriKind.Absolute);

                if (!isEnabled)
                    return String.Empty;

                Guid serviceId = Guid.NewGuid();
                string consulServiceID = $"{serviceName}:{serviceId}";

                var client = scope.ServiceProvider.GetService<IConsulClient>();

                var consulServiceRegistration = new AgentServiceRegistration
                {
                    Name = serviceName,
                    ID = consulServiceID,
                    Address = appUrl.Host,
                    Port = appUrl.Port
                };

                client.Agent.ServiceRegister(consulServiceRegistration);
                appLifetime.ApplicationStopping.Register(() =>
                {
                    client.Agent.ServiceDeregister(consulServiceRegistration.ID);
                });

                return consulServiceID;
            }
        }
    }
}