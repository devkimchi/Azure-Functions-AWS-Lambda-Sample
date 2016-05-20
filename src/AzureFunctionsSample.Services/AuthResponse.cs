using Newtonsoft.Json;

namespace AzureFunctionsSample.Services
{
    public class AuthResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public string ExpiresIn { get; set; }
    }
}