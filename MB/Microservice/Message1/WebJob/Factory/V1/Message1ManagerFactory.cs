﻿using MB.Access.Tenant.Interface.V1;
using MB.Manager.Message1.Interface.V1;
using MB.Manager.Message1.Service.V1;
using Microsoft.Extensions.Logging;

namespace MB.Microservice.Message1.WebJob.Factory.V1
{
    public class Message1ManagerFactory : IMessage1ManagerFactory
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IMessage1ManagerEvents _message1ManagerEvents;
        private readonly ITenantAccess _tenantAccess;

        public Message1ManagerFactory(ITenantAccess tenantAccess, IMessage1ManagerEvents message1ManagerEvents, ILoggerFactory loggerFactory)
        {
            _tenantAccess = tenantAccess;
            _message1ManagerEvents = message1ManagerEvents;
            _loggerFactory = loggerFactory;
        }

        public Message1Manager Create()
        {
            var logger = _loggerFactory.CreateLogger<Message1Manager>();

            return new Message1Manager(_tenantAccess, _message1ManagerEvents, logger);
        }
    }
}
