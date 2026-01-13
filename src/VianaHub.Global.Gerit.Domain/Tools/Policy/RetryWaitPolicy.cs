using Polly;
using Polly.Extensions.Http;
using Polly.Retry;

namespace VianaHub.Global.Gerit.Domain.Tools.Policy;

public static class RetryWaitPolicy
{
    public static AsyncRetryPolicy<HttpResponseMessage> GetRetryWaitPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(new[]
            {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(2),
                    TimeSpan.FromSeconds(3),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(10),
            }, (outcome, timeSpan, retryCount, contex) =>
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Trying for {retryCount} time!");
                Console.ForegroundColor = ConsoleColor.White;
            });
    }
}

