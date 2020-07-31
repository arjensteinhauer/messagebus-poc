using System;

namespace MB.Client.Gateway.Service.Hubs.V1
{
    [Serializable]
    public class Message2ConnectionId
    {
        public const string HeaderKey = "Message2-Connection-Id";
        public static readonly string ItemKey = typeof(Message2ConnectionId).Name;

#pragma warning disable CA2235 // Mark all non-serializable fields
        public string Value { get; }
#pragma warning restore CA2235 // Mark all non-serializable fields

        public Message2ConnectionId(string value)
        {
            Value = value;
        }
    }
}