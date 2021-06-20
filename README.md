# HolidayOptimizer

This API wrapper around the Nager.Date API. 

## Description

With Polly, I added resilience and transient-fault-handling for Nager API. Currently, there is only in-memory cache support. 

## Settings

#### AppSettings

Settings to be used at the application level. 

```c#
    public class AppSettings
    {
        public string[] SupportedCountryCodes { get; set; }
        public TimeSpan CacheTtl { get; set; }
        public int CacheSizeLimit { get; set; }
    }
```

#### NagerApiSettings

Settings used for the Nager API.

```c#
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
```

## TODO

- Implement distributed cache

