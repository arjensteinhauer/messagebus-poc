using System;

namespace MB.Messaging
{
    [Serializable]
    public class CorrelationId
    {
        public const string CorrelationIdHttpHeaderKey = "Correlation-Id";

#pragma warning disable CA2235 // Mark all non-serializable fields
        public string Value { get; }
#pragma warning restore CA2235 // Mark all non-serializable fields

        public CorrelationId(string value)
        {
            Value = value;
        }
    }
}
