using MB.Manager.Message2.Interface.V1;
using MB.Manager.Message2.Service.V1;
using Microsoft.Extensions.Logging;

namespace MB.Microservice.Message2.WebJob.Factory.V1
{
    public class Message2ManagerFactory : IMessage2ManagerFactory
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IMessage2ManagerEvents _message2ManagerEvents;

        public Message2ManagerFactory(IMessage2ManagerEvents message2ManagerEvents, ILoggerFactory loggerFactory)
        {
            _message2ManagerEvents = message2ManagerEvents;
            _loggerFactory = loggerFactory;
        }

        public Message2Manager Create()
        {
            var logger = _loggerFactory.CreateLogger<Message2Manager>();

            return new Message2Manager(_message2ManagerEvents, logger);
        }
    }
}
