#r "Newtonsoft.Json"

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Newtonsoft.Json;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info($"C# HTTP trigger function processed a request. RequestUri={req.RequestUri}");

    dynamic body = await req.Content.ReadAsAsync<object>().ConfigureAwait(false);
    
    // Gets the API key from https://dev.telstra.com
    var clientId = "[CLIENT_ID OR CONSUMER_KEY]";
    var clientSecret = "[CLIENT_SECRET OR CONSUMER_SECRET]";
    var grantType = "client_credentials";
    var scope = "SMS";
    var mobile = "[MOBILE_NUMBER]";
    var message = $"Push made by {body.head_commit.author.name} with commitId, {body.head_commit.id}";

    dynamic context;
    using (var client = new HttpClient() { BaseAddress = new Uri("https://api.telstra.com") })
    {
        var content = new FormUrlEncodedContent(
                new[]
                    {
                        new KeyValuePair<string, string>("client_id", clientId),
                        new KeyValuePair<string, string>("client_secret", clientSecret),
                        new KeyValuePair<string, string>("grant_type", grantType),
                        new KeyValuePair<string, string>("scope", scope),
                    });
        var messgaeResponse = await client.PostAsync("v1/oauth/token", content).ConfigureAwait(false);
        context = await messgaeResponse.Content.ReadAsAsync<object>().ConfigureAwait(false);
    }
    
    string serialised;
    using (var client = new HttpClient() { BaseAddress = new Uri("https://api.telstra.com") })
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", (string)context.access_token);
        
        var rqst = new { to = mobile, body = message };
        var content = new StringContent(JsonConvert.SerializeObject(rqst));
        var messgaeResponse = await client.PostAsync("v1/sms/messages", content).ConfigureAwait(false);
        var result = await messgaeResponse.Content.ReadAsAsync<object>().ConfigureAwait(false);

        serialised = JsonConvert.SerializeObject(result); 
        log.Info(serialised);
    }
    
    return req.CreateResponse(HttpStatusCode.OK, serialised);
}
