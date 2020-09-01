using MassTransit;
using MB.Manager.Message1.Interface.V1;
using MB.Manager.Message1.Service.V1;
using MB.Microservice.Message1.WebJob.Factory.V1;
using System.Threading.Tasks;
using AmAliveEventData = MB.Client.Gateway.Interface.V1.AmAliveEventData;
using EchoRequest = MB.Manager.Message1.Interface.V1.EchoRequest;
using EchoResponse = MB.Manager.Message1.Interface.V1.EchoResponse;

namespace MB.Microservice.Message1.WebJob.Consumers.V1
{
    public class Message1Consumer :
        IConsumer<EchoRequest>,
        IConsumer<OneWayCommand>,
        IConsumer<RequestResponseRequest>,
        IConsumer<TriggerPublishSubscribeRequest>,
        IConsumer<AmAliveEventData>
    {
        private readonly Message1Manager _message1Manager;

        public Message1Consumer(IMessage1ManagerFactory message1ManagerFactory)
        {
            _message1Manager = message1ManagerFactory.Create();
        }

        public async Task Consume(ConsumeContext<EchoRequest> context)
        {
            var result = await _message1Manager.Echo(context.Message);

            await context.RespondAsync<EchoResponse>(result);
        }

        public Task Consume(ConsumeContext<OneWayCommand> context)
        {
            return _message1Manager.OneWay(context.Message);
        }

        public async Task Consume(ConsumeContext<RequestResponseRequest> context)
        {
            var result = await _message1Manager.RequestResponse(context.Message);

            await context.RespondAsync<RequestResponseResponse>(result);
        }

        public Task Consume(ConsumeContext<TriggerPublishSubscribeRequest> context)
        {
            return _message1Manager.TriggerPublishSubscribe(context.Message);
        }

        public async Task Consume(ConsumeContext<AmAliveEventData> context)
        {
            await _message1Manager.OnAmAlive(context.Message);
        }
    }
}