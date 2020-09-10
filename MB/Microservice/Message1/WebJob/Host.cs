using MassTransit;
using MB.Access.Tenant.Database;
using MB.Access.Tenant.Interface.V1;
using MB.Access.Tenant.Service.V1;
using MB.Manager.Message1.Interface.V1;
using MB.Microservice.Message1.WebJob.Consumers.V1;
using MB.Microservice.Message1.WebJob.Factory.V1;
using MB.Utilities;
using MB.Utilities.AccessTokenProvider;
using MB.Utilities.Extensions;
using MB.Utilities.MessageBus;
using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MB.Microservice.Message1.WebJob
{
    public static class Host
    {
        private static async Task Main(string[] args)
        {
            if (!Console.IsOutputRedirected)
            {
                Console.Title = "Message1 Service";
            }

            // configure the host builder (configuration, logging and dependency injection)
            var builder = new HostBuilder()
                .ConfigureAppConfiguration(configurationBuilder => AddConfiguration(configurationBuilder))
                .ConfigureLogging((context, loggingBuilder) =>
                {
                    loggingBuilder.AddConsole();

                    var appInsightsInstrumentationKey = context.Configuration["APPINSIGHTS_INSTRUMENTATIONKEY"];
                    if (!string.IsNullOrWhiteSpace(appInsightsInstrumentationKey))
                    {
                        loggingBuilder.AddApplicationInsights();
                    }

                    loggingBuilder.AddConfiguration(context.Configuration.GetSection("Logging"));
                })
                .ConfigureServices(services => ConfigureServices(services))
                .UseConsoleLifetime();

            // build the host instance
            using (var host = builder.Build())
            {
                // start the message bus
                var messageBus = host.Services.GetService<IBusControl>();
                await messageBus.StartAsync();
                try
                {
                    // start the host and wait until Ctrl+C (or other cancelation event)
                    await host.RunAsync();
                }
                finally
                {
                    // always stop the message bus
                    await messageBus.StopAsync();
                }
            }
        }

        /// <summary>
        /// Add all configuration sources.
        /// </summary>
        /// <param name="configurationBuilder">Configuration builder to add all sources to.</param>
        private static void AddConfiguration(IConfigurationBuilder configurationBuilder)
        {
            // default appsettings file and environment variables
            configurationBuilder
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                .AddEnvironmentVariables();

            // for local development add local development settings
            if (ConfigurationBuilderExtensions.IsLocalDevelopment())
            {
                var configuration = configurationBuilder.Build();
                var environmentName = configuration["WEBJOB_ENVIRONMENT"];
                configurationBuilder
                    .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: false)
                    .AddUserSecrets(typeof(Host).Assembly, true, false);
            }

            // for production add keyvault settings
            configurationBuilder
                .AddConfiguration();
        }

        /// <summary>
        /// Configure the services dependency injection.
        /// </summary>
        /// <param name="services">Service collection from the DI container.</param>
        private static void ConfigureServices(IServiceCollection services)
        {
            // application insights telemetry
            if (!ConfigurationBuilderExtensions.IsLocalDevelopment())
            {
                services.AddApplicationInsightsTelemetryWorkerService();
                services.ConfigureTelemetryModule<DependencyTrackingTelemetryModule>((module, options) =>
                {
                    module.IncludeDiagnosticSourceActivities.Add("MassTransit");
                });
            }

            // utility services
            services.AddSingleton<IAccessTokenProvider>((provider) => ConfigurationBuilderExtensions.IsLocalDevelopment() ? null : new AccessTokenProvider());

            // tenant access service
            services.AddDbContext<TenantContext>();
            services.AddScoped<ITenantContextFactory<TenantContext>, TenantContextFactory>();
            services.AddScoped<ITenantAccess, TenantAccess>();

            // notification manager service
            services.AddScoped<IMessage1ManagerFactory, Message1ManagerFactory>();
            services.AddScoped<IMessage1ManagerEvents, Message1ManagerEventsPublisher>();

            // configure message bus
            services.AddMassTransit(configure =>
            {
                configure.AddConsumersFromNamespaceContaining<ConsumerNamespace>();
                configure.AddBus(context => BusControlFactory.Create(context));
            });
        }
    }
}
