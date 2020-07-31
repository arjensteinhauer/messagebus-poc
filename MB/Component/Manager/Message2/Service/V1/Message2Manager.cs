﻿using MB.Manager.Message2.Interface.V1;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using AmAliveEventData_External = MB.Client.Gateway.Interface.V1.AmAliveEventData;
using EchoResponse = MB.Manager.Message2.Interface.V1.EchoResponse;
using IAmAliveEvent_External = MB.Client.Gateway.Interface.V1.IAmAliveEvent;

namespace MB.Manager.Message2.Service.V1
{
    public class Message2Manager : IMessage2Manager, IAmAliveEvent_External
    {
        private readonly IMessage2ManagerEvents _message1ManagerEvents;
        private readonly ILogger _logger;

        public Message2Manager(IMessage2ManagerEvents message1ManagerEvents, ILogger<Message2Manager> logger)
        {
            _message1ManagerEvents = message1ManagerEvents;
            _logger = logger;
        }

        public async Task<EchoResponse> Echo(Interface.V1.EchoRequest request)
        {
            var result = $"{request.Input} -> \t {Environment.MachineName}.{GetType().Namespace}.{GetType().Name}.{nameof(Echo)} \n";
            _logger.LogInformation(result);

            await _message1ManagerEvents.OnEchoed(new EchoedEventData { Result = result });

            return new EchoResponse { Result = result };
        }

        public async Task OnAmAlive(AmAliveEventData_External eventData)
        {
            var result = $"Received message '{eventData.Message}'. {Environment.MachineName}.{GetType().Namespace}.{GetType().Name}.{nameof(OnAmAlive)} is alive too.";
            _logger.LogInformation(result);

            await _message1ManagerEvents.OnAmAlive(new AmAliveEventData { Message = result });
        }
    }
}