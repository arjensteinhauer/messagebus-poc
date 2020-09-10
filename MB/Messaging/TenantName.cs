using System;

namespace MB.Messaging
{
    [Serializable]
    public class TenantName
    {
        public const string TenantNameHttpHeaderKey = "TenantName";

#pragma warning disable CA2235 // Mark all non-serializable fields
        public string Value { get; }
#pragma warning restore CA2235 // Mark all non-serializable fields

        public TenantName(string value)
        {
            Value = value;
        }
    }
}
