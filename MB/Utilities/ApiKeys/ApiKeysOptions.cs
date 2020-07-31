namespace MB.Utilities.ApiKeys
{
    public class ApiKeysOptions
    {
        public const string ApiKeys = "ApiKeys";

        public ApiKey[] Keys { get; set; }
    }

    public class ApiKey
    {
        public string Key { get; set; }
    }
}
