using MB.Client.Gateway.Service.Controllers.V1.Mapping;
using MB.Manager.Message1.Interface.V1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MB.Client.Gateway.Service.Controllers.V1
{
    [ApiController]
    [Route("api/v1")]
    public class Message1Controller : Message1ControllerBase
    {
        private readonly ILogger<Message1Controller> logger;
        private readonly IMessage1Manager message1Manager;

        public Message1Controller(ILogger<Message1Controller> logger, IMessage1Manager message1Manager)
        {
            this.message1Manager = message1Manager;
            this.logger = logger;
        }

        public override async Task<string> Echo([BindRequired, FromBody] EchoRequest body)
        {
            var managerModel = body.Message1Map();
            var managerResponse = await message1Manager.Echo(managerModel);
            return managerResponse.Map();
        }

        public override async Task<string> OneWay([BindRequired, FromBody] OneWayRequest body)
        {
            var managerModel = body.Message1Map();
            await message1Manager.OneWay(managerModel);
            return $"Message '{body.Input}' delivered to the Message Bus...";
        }

        public override async Task<string> RequestResponse([BindRequired, FromBody] RequestResponseRequest body)
        {
            var managerModel = body.Message1Map();
            var managerResponse = await message1Manager.RequestResponse(managerModel);
            return managerResponse.Map();
        }
    }
}
