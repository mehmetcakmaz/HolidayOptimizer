using System;

namespace HolidayOptimizer.API.Settings
{
    public class NagerApiSettings
    {
        public string ApiUrl { get; set; }

        /// <summary>
        /// TimeOut value for every request. With polly we use the retry policy for timeout. 
        /// </summary>
        public TimeSpan PollyTimeOut { get; set; }

        /// <summary>
        /// Timeout value for HttpClient.
        /// </summary>
        public TimeSpan HttpClientTimeout { get; set; }

        /// <summary>
        /// https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests#httpclient-lifetimes
        /// </summary>
        public TimeSpan HandlerLifeTime { get; set; }
    }
}
