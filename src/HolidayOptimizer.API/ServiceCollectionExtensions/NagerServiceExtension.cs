using HolidayOptimizer.API.Services;
using HolidayOptimizer.API.Services.Implementations;
using HolidayOptimizer.API.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HolidayOptimizer.API.ServiceCollectionExtensions
{
    public static class NagerServiceExtension
    {
        public static void AddNagerServiceHttpClient(this IServiceCollection serviceCollection, NagerApiSettings nagerApiSettings)
        {
            serviceCollection.AddHttpClient<INagerService, NagerService>()
                .ConfigureHttpClient(c =>
                {
                    c.Timeout = nagerApiSettings.HttpClientTimeout;
                    c.BaseAddress = new Uri(nagerApiSettings.ApiUrl);
                })
                .SetHandlerLifetime(nagerApiSettings.HandlerLifeTime)
                .AddPolicyHandler((service, request) => HttpPolicyExtensions.HandleTransientHttpError().Or<TimeoutRejectedException>()
                    .WaitAndRetryAsync(new[]
                        {
                            TimeSpan.FromMilliseconds(100),
                            TimeSpan.FromMilliseconds(100),
                            TimeSpan.FromMilliseconds(100)
                        },
                        onRetry: (result, timeSpan, retryAttempt, context) => { OnRetry(service, result, timeSpan, retryAttempt, context); }
                    )
                    .WrapAsync(HttpPolicyExtensions.HandleTransientHttpError().AdvancedCircuitBreakerAsync(
                        failureThreshold: 0.5,
                        samplingDuration: TimeSpan.FromSeconds(5),
                        minimumThroughput: 5,
                        durationOfBreak: TimeSpan.FromSeconds(10)))
                    .WrapAsync(Policy.TimeoutAsync<HttpResponseMessage>(
                        timeout: nagerApiSettings.PollyTimeOut,
                        onTimeoutAsync: (context, timeSpan, task) => OnTimeoutAsync(service, context, timeSpan, task))));
        }

        private static void OnRetry(IServiceProvider service, DelegateResult<HttpResponseMessage> result, TimeSpan timeSpan, int retryAttempt, Context context)
        {
            var logger = service.GetService<ILogger<NagerService>>();
            if (result.Result != null)
            {
                logger.LogWarning(
                    $"{nameof(NagerService)} Request failed with {result.Result.StatusCode}. Waiting {timeSpan.TotalMilliseconds} ms before next retry. Retry attempt {retryAttempt}");
            }
            else
            {
                logger.LogWarning($"{nameof(NagerService)} Request failed because network failure. Waiting {timeSpan.TotalMilliseconds} ms before next retry. Retry attempt {retryAttempt}");
            }
        }

        private static Task OnTimeoutAsync(IServiceProvider service, Context context, TimeSpan timeSpan, Task task)
        {
            var logger = service.GetService<ILogger<NagerService>>();
            logger.LogWarning($"Execution timed out after {timeSpan.TotalSeconds} seconds, task cancelled.");

            return Task.FromResult(0);
        }
    }
}
