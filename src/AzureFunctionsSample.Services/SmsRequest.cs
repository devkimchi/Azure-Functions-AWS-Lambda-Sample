using Newtonsoft.Json;

namespace AzureFunctionsSample.Services
{
    public class SmsRequest
    {
        public SmsRequest()
        {
            this.GrantType = Services.GrantType.ClientCredentials;
            this.Scope = ScopeType.Sms;
        }

        [JsonIgnore]
        public string ClientId { get; set; }

        [JsonIgnore]
        public string ClientSecret { get; set; }

        [JsonIgnore]
        public string GrantType { get; set; }

        [JsonIgnore]
        public string Scope { get; set; }

        [JsonProperty("to")]
        public string Mobile { get; set; }

        [JsonProperty("body")]
        public string Message { get; set; }
    }
}