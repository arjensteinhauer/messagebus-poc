using MB.Access.Tenant.Interface.V1;
using MB.Manager.Message1.Interface.V1;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using AmAliveEventData_External = MB.Client.Gateway.Interface.V1.AmAliveEventData;
using EchoResponse = MB.Manager.Message1.Interface.V1.EchoResponse;
using IAmAliveEvent_External = MB.Client.Gateway.Interface.V1.IAmAliveEvent;

namespace MB.Manager.Message1.Service.V1
{
    public class Message1Manager : IMessage1Manager, IAmAliveEvent_External
    {
        private readonly ITenantAccess _tenantAccess;
        private readonly IMessage1ManagerEvents _message1ManagerEvents;
        private readonly ILogger _logger;

        public Message1Manager(ITenantAccess tenantAccess, IMessage1ManagerEvents message1ManagerEvents, ILogger<Message1Manager> logger)
        {
            _tenantAccess = tenantAccess;
            _message1ManagerEvents = message1ManagerEvents;
            _logger = logger;
        }

        public async Task<EchoResponse> Echo(Interface.V1.EchoRequest request)
        {
            var result = $"{request.Input} -> \t {Environment.MachineName}.{GetType().Namespace}.{GetType().Name}.{nameof(Echo)} \n";
            _logger.LogInformation(result);

            result = await _tenantAccess.Echo($"{result} \t\t");

            await _message1ManagerEvents.OnEchoed(new EchoedEventData { Result = result });

            return new EchoResponse { Result = result };
        }

        public async Task OneWay(OneWayCommand command)
        {
            var result = $"{command.Input} -> {Environment.MachineName}.{GetType().Namespace}.{GetType().Name}.{nameof(OneWay)} \n";
            _logger.LogInformation(result);

            await _tenantAccess.StoreMessage(new StoreMessageRequest { MessageDetails = new Message { Subject = "Message1.OneWay", Body = result } });

            await _message1ManagerEvents.OnProcessedOneWayCommand(new ProcessedOneWayCommandEventData { Result = result });
        }

        public async Task<RequestResponseResponse> RequestResponse(RequestResponseRequest request)
        {
            var result = $"{request.Input} -> {Environment.MachineName}.{GetType().Namespace}.{GetType().Name}.{nameof(RequestResponse)}";
            _logger.LogInformation(result);

            await _tenantAccess.StoreMessage(new StoreMessageRequest { MessageDetails = new Message { Subject = "Message1.RequestResponse", Body = result } });

            return await Task.FromResult(new RequestResponseResponse { Result = result });
        }

        public async Task TriggerPublishSubscribe(TriggerPublishSubscribeRequest request)
        {
            var result = $"{Environment.MachineName}.{GetType().Namespace}.{GetType().Name}.{nameof(TriggerPublishSubscribe)} for '{request.Name}'.";
            _logger.LogInformation(result);

            await _tenantAccess.StoreMessage(new StoreMessageRequest { MessageDetails = new Message { Subject = "Message1.TriggerPublishSubscribe", Body = result } });

            await _message1ManagerEvents.OnPublishSomething(new PublishSomethingEventData { Name = request.Name, Message = $"Some message from {Environment.MachineName} published on {DateTime.Now:dd-MM-yyyy HH:mm:ss}" });
        }

        public async Task OnAmAlive(AmAliveEventData_External eventData)
        {
            var result = $"Received message '{eventData.Message}'. {Environment.MachineName}.{GetType().Namespace}.{GetType().Name}.{nameof(OnAmAlive)} is alive too.";
            _logger.LogInformation(result);

            await _tenantAccess.StoreMessage(new StoreMessageRequest { MessageDetails = new Message { Subject = "Message1.OnAmAlive", Body = result } });

            await _message1ManagerEvents.OnAmAlive(new AmAliveEventData { Message = result });
        }
    }
}
