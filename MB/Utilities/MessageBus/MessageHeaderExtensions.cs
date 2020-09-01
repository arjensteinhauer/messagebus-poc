using MassTransit;
using System.Linq;

namespace MB.Utilities.MessageBus
{
    public static class MessageHeaderExtensions
    {
        public static void AddToMassTransitMessage(this SendHeaders headers, MessageHeaders messageHeaders)
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


        public static MessageHeaders FromMassTransitMessage(this Headers headers)
        {
            if (headers == null)
            {
                return new MessageHeaders();
            }

            return new MessageHeaders(
                headers
                    .GetAll()
                    .Where(header => header.Key.StartsWith(MessageHeaders.HeaderKeyPrefix))
                    .ToDictionary(header => header.Key, header => header.Value));
        }
    }
}
