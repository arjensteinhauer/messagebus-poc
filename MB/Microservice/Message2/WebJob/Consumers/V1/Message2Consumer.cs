using MassTransit;
using MB.Manager.Message2.Service.V1;
using MB.Microservice.Message2.WebJob.Factory.V1;
using System.Threading.Tasks;
using AmAliveEventData = MB.Client.Gateway.Interface.V1.AmAliveEventData;
using EchoRequest = MB.Manager.Message2.Interface.V1.EchoRequest;
using EchoResponse = MB.Manager.Message2.Interface.V1.EchoResponse;

namespace MB.Microservice.Message2.WebJob.Consumers.V1
{
    public class Message2Consumer : IConsumer<EchoRequest>, IConsumer<AmAliveEventData>
    {
        private readonly Message2Manager _message2Manager;

        public Message2Consumer(IMessage2ManagerFactory message2ManagerFactory)
        {
            _message2Manager = message2ManagerFactory.Create();
        }

        public async Task Consume(ConsumeContext<EchoRequest> context)
        {
            var result = await _message2Manager.Echo(context.Message);

            await context.RespondAsync<EchoResponse>(result);
        }

        public async Task Consume(ConsumeContext<AmAliveEventData> context)
        {
            await _message2Manager.OnAmAlive(context.Message);
        }
    }
}