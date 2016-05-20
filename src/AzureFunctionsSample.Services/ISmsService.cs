using System;
using System.Threading.Tasks;

namespace AzureFunctionsSample.Services
{
    /// <summary>
    /// This provides interfaces to the <see cref="SmsService"/> class.
    /// </summary>
    public interface ISmsService : IDisposable
    {
        /// <summary>
        /// Gets the <see cref="AuthResponse"/> instance containing access token.
        /// </summary>
        /// <param name="request"><see cref="SmsRequest"/> instance.</param>
        /// <returns>Returns the <see cref="AuthResponse"/> instance.</returns>
        Task<AuthResponse> GetAccessTokenAsync(SmsRequest request);

        /// <summary>
        /// Sends the SMS message.
        /// </summary>
        /// <param name="request"><see cref="SmsRequest"/> instance.</param>
        /// <returns>Returns the <see cref="SmsResponse"/> instance.</returns>
        Task<SmsResponse> SendMessageAsync(SmsRequest request);
    }
}