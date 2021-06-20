using System;

namespace HolidayOptimizer.API.Settings
{
    public class AppSettings
    {
        public string[] SupportedCountryCodes { get; set; }
        public TimeSpan CacheTtl { get; set; }
        public int CacheSizeLimit { get; set; }
    }
}
