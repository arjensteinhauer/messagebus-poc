using System;

namespace MB.Utilities
{
    public class Config
    {
        public bool UseAzureSignalR
        {
            get
            {
                var useAzureSignalR = Environment.GetEnvironmentVariable("USE_AZURE_SIGNALR");
                if (bool.TryParse(useAzureSignalR, out bool value))
                {
                    return value;
                }

                return false;
            }
        }
    }
}
