using MassTransit;
using System.Linq;

namespace MB.Utilities.Extensions
{
    public static class MessageHeaderExtensions
    {
        public static void AddToMassTransitMessage(this SendHeaders headers, MessageBus.MessageHeaders messageHeaders)
        {
            if (headers == null || messageHeaders == null)
            {
                return;
            }

            foreach (var messageHeader in messageHeaders)
            {
                headers.Set(messageHeader.Key, messageHeader.Value, overwrite: true);
            }
        }


        public static MessageBus.MessageHeaders FromMassTransitMessage(this Headers headers)
        {
            if (headers == null)
            {
                return new MessageBus.MessageHeaders();
            }

            return new MessageBus.MessageHeaders(
                headers
                    .GetAll()
                    .Where(header => header.Key.StartsWith(MessageBus.MessageHeaders.HeaderKeyPrefix))
                    .ToDictionary(header => header.Key, header => header.Value));
        }
    }
}
