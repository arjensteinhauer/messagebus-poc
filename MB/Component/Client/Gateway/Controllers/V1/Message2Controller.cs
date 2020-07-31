using MB.Client.Gateway.Service.Controllers.V1.Mapping;
using MB.Manager.Message2.Interface.V1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MB.Client.Gateway.Service.Controllers.V1
{
    [ApiController]
    [Route("api/v1")]
    public class Message2Controller : Message2ControllerBase
    {
        private readonly ILogger<Message2Controller> logger;
        private readonly IMessage2Manager message2Manager;

        public Message2Controller(ILogger<Message2Controller> logger, IMessage2Manager message2Manager)
        {
            this.message2Manager = message2Manager;
            this.logger = logger;
        }

        public override async Task<string> Echo([BindRequired, FromBody] EchoRequest body)
        {
            var managerModel = body.Message2Map();
            var managerResponse = await message2Manager.Echo(managerModel);
            return managerResponse.Map();
        }
    }
}
