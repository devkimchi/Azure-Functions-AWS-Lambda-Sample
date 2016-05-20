using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace AzureFunctionsSample.Services
{
    /// <summary>
    /// This represents the service entity for SMS.
    /// </summary>
    public class SmsService : ISmsService
    {
        private bool _disposed;

        /// <summary>
        /// Gets the <see cref="AuthResponse"/> instance containing access token.
        /// </summary>
        /// <param name="request"><see cref="SmsRequest"/> instance.</param>
        /// <returns>Returns the <see cref="AuthResponse"/> instance.</returns>
        public async Task<AuthResponse> GetAccessTokenAsync(SmsRequest request)
        {
            using (var client = new HttpClient() { BaseAddress = new Uri("https://api.telstra.com") })
            {
                var content = new FormUrlEncodedContent(
                        new[]
                            {
                                new KeyValuePair<string, string>("client_id", request.ClientId),
                                new KeyValuePair<string, string>("client_secret", request.ClientSecret),
                                new KeyValuePair<string, string>("grant_type", request.GrantType),
                                new KeyValuePair<string, string>("scope", request.Scope),
                            });
                var message = await client.PostAsync("v1/oauth/token", content).ConfigureAwait(false);
                var result = await message.Content.ReadAsStringAsync().ConfigureAwait(false);

                var response = JsonConvert.DeserializeObject<AuthResponse>(result);
                return response;
            }
        }

        /// <summary>
        /// Sends the SMS message.
        /// </summary>
        /// <param name="request"><see cref="SmsRequest"/> instance.</param>
        /// <returns>Returns the <see cref="SmsResponse"/> instance.</returns>
        public async Task<SmsResponse> SendMessageAsync(SmsRequest request)
        {
            var context = await this.GetAccessTokenAsync(request).ConfigureAwait(false);

            using (var client = new HttpClient() { BaseAddress = new Uri("https://api.telstra.com") })
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

                var content = new StringContent(JsonConvert.SerializeObject(request));
                var message = await client.PostAsync("v1/sms/messages", content).ConfigureAwait(false);
                var result = await message.Content.ReadAsStringAsync().ConfigureAwait(false);

                var response = JsonConvert.DeserializeObject<SmsResponse>(result);
                return response;
            }
        }

        public void Dispose()
        {
            if (this._disposed)
            {
                return;
            }

            this._disposed = true;
        }
    }
}