#r "Newtonsoft.Json.dll"
#r "AzureFunctionsSample.Services.dll"

using System;
using System.Net;
using System.Net.Http.Formatting;

using AzureFunctionsSample.Services;

using Newtonsoft.Json;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info($"C# HTTP trigger function processed a request. RequestUri={req.RequestUri}");

    dynamic body = await req.Content.ReadAsAsync<object>().ConfigureAwait(false);

    // Gets the API key from https://dev.telstra.com
    var request = new SmsRequest()
                      {
                          ClientId = "[CLIENT_ID OR CONSUMER_KEY]",
                          ClientSecret = "[CLIENT_SECRET OR CONSUMER_SECRET]",
                          Mobile = "[MOBILE_NUMBER]",
                          Message = $"Push made by {body.head_commit.author.name} with commitId, {body.head_commit.id}"
                      };
    
    using (var service = new SmsService())
    {
        var response = await service.SendMessageAsync(request).ConfigureAwait(false);
        
        var serialsed = JsonConvert.SerializeObject(response);
        log.Info(serialsed);
        return req.CreateResponse(HttpStatusCode.OK, response);
    }
}