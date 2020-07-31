using MB.Client.Desktop.App.EventHandlers;
using MB.Client.Desktop.App.Proxies;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Refit;
using System;
using System.IO;
using System.Windows;

namespace MB.Client.Desktop.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = new HostBuilder()
                .ConfigureAppConfiguration(configurationBuilder => AddConfiguration(configurationBuilder))
                .ConfigureLogging(loggingBuilder => loggingBuilder.AddConsole())
                .ConfigureServices(services => ConfigureServices(services))
                .Build();
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
            configurationBuilder
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: false)
                .AddUserSecrets(typeof(App).Assembly, true, false);
        }

        /// <summary>
        /// Configure the dependency injection container.
        /// </summary>
        /// <param name="services">Service collection to configure.</param>
        private void ConfigureServices(IServiceCollection services)
        {
            // main window instance
            services.AddSingleton<MainWindow>();

            // http API clients for the API gateway
            services.AddRefitClient<IMessage1Service>()
                .ConfigureHttpClient((provider, httpClient) =>
                {
                    var configuration = provider.GetService<IConfiguration>();
                    httpClient.BaseAddress = new Uri(configuration.GetConnectionString("ApiGatewayBaseUrl"));
                });
            services.AddRefitClient<IMessage2Service>()
                .ConfigureHttpClient((provider, httpClient) =>
                {
                    var configuration = provider.GetService<IConfiguration>();
                    httpClient.BaseAddress = new Uri(configuration.GetConnectionString("ApiGatewayBaseUrl"));
                });
            services.AddRefitClient<IEventsService>()
                .ConfigureHttpClient((provider, httpClient) =>
                {
                    var configuration = provider.GetService<IConfiguration>();
                    httpClient.BaseAddress = new Uri(configuration.GetConnectionString("ApiGatewayBaseUrl"));
                });

            // SignalR connections for handling API gateway events
            services.AddSingleton<IMessage1EventHandler>(provider =>
            {
                var configuration = provider.GetService<IConfiguration>();
                HubConnection connection = new HubConnectionBuilder()
                    .WithUrl($"{configuration.GetConnectionString("ApiGatewayBaseUrl")}/message1hub")
                    .WithAutomaticReconnect()
                    .Build();

                return new Message1EventHandler(connection);
            });
            services.AddSingleton<IMessage2EventHandler>(provider =>
            {
                var configuration = provider.GetService<IConfiguration>();
                HubConnection connection = new HubConnectionBuilder()
                    .WithUrl($"{configuration.GetConnectionString("ApiGatewayBaseUrl")}/message2hub")
                    .WithAutomaticReconnect()
                    .Build();

                return new Message2EventHandler(connection);
            });
        }

        /// <summary>
        /// Event called when starting the app.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            // start host
            await _host.StartAsync();

            // show the main window on start
            var mainWindow = _host.Services.GetService<MainWindow>();
            mainWindow.Show();
        }

        private async void Application_Exit(object sender, ExitEventArgs e)
        {
            using (_host)
            {
                await _host.StopAsync();
            }
        }
    }
}
