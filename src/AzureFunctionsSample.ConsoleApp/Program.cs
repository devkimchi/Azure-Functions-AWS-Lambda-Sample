using System;
using System.Linq;

using AzureFunctionsSample.Services;

namespace AzureFunctionsSample.ConsoleApp
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var request = GetRequest(args);
            using (var service = new SmsService())
            {
                var response = service.SendMessageAsync(request).Result;

                Console.WriteLine($"MessageId: {response.MessageId}");
            }
        }

        private static SmsRequest GetRequest(string[] args)
        {
            var clientId = args.ToArgument("/id:");
            var clientSecret = args.ToArgument("/secret:");
            var mobile = args.ToArgument("/mobile:");
            var message = args.ToArgument("/message:");

            var request = new SmsRequest()
                              {
                                  ClientId = clientId,
                                  ClientSecret = clientSecret,
                                  Mobile = mobile,
                                  Message = message
                              };
            return request;
        }

        private static string ToArgument(this string[] args, string parameter)
        {
            var result =
                args.Single(p => p.ToLowerInvariant().StartsWith(parameter))
                    .Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[1];
            return result;
        }
    }
}