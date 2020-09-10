using MassTransit;
using MB.Client.Gateway.Service.EventProxies.V1;
using MB.Client.Gateway.Service.Hubs.V1;
using MB.Client.Gateway.Service.Middleware;
using MB.Manager.Message1.Interface.V1;
using MB.Manager.Message1.Proxy.V1;
using MB.Manager.Message2.Interface.V1;
using MB.Manager.Message2.Proxy.V1;
using MB.Utilities;
using MB.Utilities.Extensions;
using MB.Utilities.MessageBus;
using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using IGatewayAmAliveEvent = MB.Client.Gateway.Interface.V1.IAmAliveEvent;
using IMessage1AmAliveEvent = MB.Manager.Message1.Interface.V1.IAmAliveEvent;
using IMessage1EchoedEvent = MB.Manager.Message1.Interface.V1.IEchoedEvent;
using IMessage2AmAliveEvent = MB.Manager.Message2.Interface.V1.IAmAliveEvent;
using IMessage2EchoedEvent = MB.Manager.Message2.Interface.V1.IEchoedEvent;
using Message1AmAliveEventConsumer = MB.Manager.Message1.Proxy.V1.AmAliveEventConsumer;
using Message1EchoedEventConsumer = MB.Manager.Message1.Proxy.V1.EchoedEventConsumer;
using Message1EchoRequest = MB.Manager.Message1.Interface.V1.EchoRequest;
using Message1EchoResponse = MB.Manager.Message1.Interface.V1.EchoResponse;
using Message2AmAliveEventConsumer = MB.Manager.Message2.Proxy.V1.AmAliveEventConsumer;
using Message2EchoedEventConsumer = MB.Manager.Message2.Proxy.V1.EchoedEventConsumer;
using Message2EchoRequest = MB.Manager.Message2.Interface.V1.EchoRequest;
using Message2EchoResponse = MB.Manager.Message2.Interface.V1.EchoResponse;

namespace MB.Client.Gateway.Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // logging
            services.AddLogging(builder =>
            {
                builder.AddConsole();
            });

            // Application Insights logging with telemetry
            if (!ConfigurationBuilderExtensions.IsLocalDevelopment())
            {
                services.AddApplicationInsightsTelemetry();
                services.AddLogging(builder =>
                {
                    builder.AddApplicationInsights();
                });
                services.ConfigureTelemetryModule<DependencyTrackingTelemetryModule>((module, options) =>
                {
                    module.IncludeDiagnosticSourceActivities.Add("MassTransit");
                });

                services.AddHsts((options) =>
                {
                    // TimeSpan in seconds (that's how it shows in the header)
                    // 15768000 seconds is half a year
                    options.MaxAge = TimeSpan.FromSeconds(15768000);
                });
            }

            // micro services
            ConfigureMicroServices(services);

            // add CORS
            services.AddCors();

            // API controllers
            services.AddControllers();

            // Open API 3.0 + Swagger 2.0 documentation
            services.AddOpenApiDocument(document =>
            {
                document.DocumentName = "V1";
                document.Title = "MB API Gateway v1";
                document.Description = "API Gateway for the Message Bus - Proof of Concept";
            });

            // signalR
            var config = new Config();
            var signalRServerBuilder = services
                .AddSignalR();

            if (config.UseAzureSignalR)
            {
                signalRServerBuilder.AddAzureSignalR();
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // enable local development extra's
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // register the Swagger generator and the Swagger UI middlewares
            app.UseOpenApi();
            app.UseSwaggerUi3();

            // only https
            app.UseHttpsRedirection();

            // routing
            app.UseRouting();

            // CORS: allow any
            app.UseCors(x =>
            {
                x.AllowAnyHeader();
                x.AllowAnyMethod();
                x.AllowAnyOrigin();
            });

            // authorization
            app.UseAuthorization();

            // custom middleware handlers
            app.UseCustomExceptionHandler();
            app.UseCorrelationIdHandler();
            app.UseTenantNameHandler();
            app.UseSignalRConnectionIdHandler();

            // endpoint mapping
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<Message1Hub>("/api/v1/message1hub");
                endpoints.MapHub<Message2Hub>("/api/v1/message2hub");
            });
        }

        private void ConfigureMicroServices(IServiceCollection services)
        {
            // message1 manager service proxy
            services.AddScoped<IMessage1Manager, Message1ManagerClient>();
            services.AddScoped<IMessage1EchoedEvent, Message1EventsHandler>();
            services.AddScoped<IProcessedOneWayCommandEvent, Message1EventsHandler>();
            services.AddScoped<IMessage1AmAliveEvent, Message1EventsHandler>();
            services.AddScoped<IPublishSomethingEvent, Message1EventsHandler>();

            // message 2 manager service proxy
            services.AddScoped<IMessage2Manager, Message2ManagerClient>();
            services.AddScoped<IMessage2EchoedEvent, Message2EventsHandler>();
            services.AddScoped<IMessage2AmAliveEvent, Message2EventsHandler>();

            // events publisher proxy
            services.AddScoped<IGatewayAmAliveEvent, EventsPublisher>();

            // configure message bus
            services.AddMassTransit(configure =>
            {
                // message 1 manager events consumers
                configure.AddConsumer<Message1EchoedEventConsumer>();
                configure.AddConsumer<ProcessedOneWayCommandEventConsumer>();
                configure.AddConsumer<Message1AmAliveEventConsumer>();

                // message 2 manager events consumers
                configure.AddConsumer<Message2EchoedEventConsumer>();
                configure.AddConsumer<Message2AmAliveEventConsumer>();

                // message bus factory
                configure.AddBus(context => BusControlFactory.Create(context));

                // message1 manager request clients
                configure.AddRequestClient<Message1EchoRequest>();
                configure.AddRequestClient<RequestResponseRequest>();

                // message2 manager request clients
                configure.AddRequestClient<Message2EchoRequest>();
            });

            services.AddScoped<IRequestResponseClient<Message1EchoRequest, Message1EchoResponse>, RequestResponseClient<Message1EchoRequest, Message1EchoResponse>>();
            services.AddScoped<IRequestResponseClient<RequestResponseRequest, RequestResponseResponse>, RequestResponseClient<RequestResponseRequest, RequestResponseResponse>>();

            services.AddScoped<IRequestResponseClient<Message2EchoRequest, Message2EchoResponse>, RequestResponseClient<Message2EchoRequest, Message2EchoResponse>>();

            // events subscriber
            services.AddSingleton<IMessageBusControl, MessageBusControl>();
            services.AddSingleton<IEventSubscriber, EventSubscriber>();

            services.AddMassTransitHostedService();
        }
    }
}
