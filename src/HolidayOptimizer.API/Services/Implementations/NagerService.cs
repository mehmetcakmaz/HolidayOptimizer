using HolidayOptimizer.API.Common;
using HolidayOptimizer.API.Model.Domain;
using HolidayOptimizer.API.Settings;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace HolidayOptimizer.API.Services.Implementations
{
    public class NagerService : INagerService
    {
        private readonly AppSettings _appSettings;
        private readonly HttpClient _client;
        private readonly IMemoryCache _cache;

        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };

        public NagerService(HttpClient client, IMemoryCache memoryCache, NagerApiSettings nagerApiSettings, AppSettings appSettings)
        {
            _client = client;
            _cache = memoryCache;
            _appSettings = appSettings;
        }

        public async Task<IEnumerable<HolidayModel>> GetPublicHolidaysForAllCountryAsync(int year)
        {
            return await _cache.GetOrCreateAsync<IEnumerable<HolidayModel>>($"{CacheKeys.PublicHolidaysForAllCountry}-{year}", entry =>
                {
                    entry.SlidingExpiration = _appSettings.CacheTtl;
                    entry.Size = 1;
                    return GetPublicHolidaysAllCountryForCacheAsync(year);
                });
        }

        public async Task<CountryModel> GetCountryInfoAsync(string countryCode)
        {
            return await _cache.GetOrCreateAsync<CountryModel>($"{CacheKeys.CountryInfo}-{countryCode}", entry =>
            {
                entry.SlidingExpiration = _appSettings.CacheTtl;
                entry.Size = 1;
                return GetCountryInfoForCacheAsync(countryCode);
            });
        }

        private async Task<IEnumerable<HolidayModel>> GetPublicHolidayAsync(string countryCode, int year)
        {
            var response = await _client.GetAsync($"PublicHolidays/{year}/{countryCode}");

            response.EnsureSuccessStatusCode();

            var contentStream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<IEnumerable<HolidayModel>>(contentStream, Options);
        }

        private async Task<IEnumerable<HolidayModel>> GetPublicHolidaysAllCountryForCacheAsync(int year)
        {
            var holidayModels = new List<HolidayModel>();
            var taskList = new List<Task>();

            foreach (var countryCode in _appSettings.SupportedCountryCodes)
            {
                taskList.Add(GetPublicHolidayAsync(countryCode, year));
            }

            await Task.WhenAll(taskList.ToArray());

            foreach (var task in taskList)
            {
                var taskResult = ((Task<IEnumerable<HolidayModel>>)task).Result;
                holidayModels.AddRange(taskResult);
            }

            return holidayModels;
        }

        private async Task<CountryModel> GetCountryInfoForCacheAsync(string countryCode)
        {
            var response = await _client.GetAsync($"CountryInfo/{countryCode}");

            response.EnsureSuccessStatusCode();

            var contentStream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<CountryModel>(contentStream, Options);
        }

    }
}
