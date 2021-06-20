using HolidayOptimizer.API.Model.Responses;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace HolidayOptimizer.API.Services.Implementations
{
    public class HolidayOptimizerService : IHolidayOptimizerService
    {
        private readonly INagerService _nagerService;

        public HolidayOptimizerService(INagerService nagerService)
        {
            _nagerService = nagerService;
        }

        public async Task<BaseResponse<GetCountryWithMostHolidaysResponse>> GetCountryWithMostHolidaysThisYear()
        {
            var response = new BaseResponse<GetCountryWithMostHolidaysResponse>()
            {
                Data = new GetCountryWithMostHolidaysResponse()
            };

            var holidays = await _nagerService.GetPublicHolidaysForAllCountryAsync(DateTime.Now.Year);

            var mostHolidayCountry = holidays.GroupBy(x => x.CountryCode).Select(x => new
            {
                countryCode = x.Key,
                count = x.Count()
            }).OrderByDescending(x => x.count).First();

            var countryModel = await _nagerService.GetCountryInfoAsync(mostHolidayCountry.countryCode);

            response.Data.CountryCode = countryModel.CountryCode;
            response.Data.OfficialName = countryModel.OfficialName;

            return response;
        }

        public async Task<BaseResponse<string>> GetMonthWithMostHolidaysByYear(int year)
        {
            var response = new BaseResponse<string>();
            if (year < DateTime.MinValue.Year)
            {
                response.Errors.Add($"{nameof(year)} parameter must be a valid value.");
            }

            if (response.HasError)
            {
                return response;
            }

            var holidays = await _nagerService.GetPublicHolidaysForAllCountryAsync(year);

            var mostHolidayMonth = holidays.GroupBy(x => x.Date.Month).Select(x => new
            {
                month = x.Key,
                count = x.Count()
            }).OrderByDescending(x => x.count).First();

            response.Data = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(mostHolidayMonth.month);

            return response;
        }

        public async Task<BaseResponse<string>> GetCountryWithMostUniqueHolidaysByYear(int year)
        {
            var response = new BaseResponse<string>()
            {
                Data = string.Empty
            };

            if (year < DateTime.MinValue.Year)
            {
                response.Errors.Add($"{nameof(year)} parameter must be a valid value.");
            }

            if (response.HasError)
            {
                return response;
            }

            var holidays = await _nagerService.GetPublicHolidaysForAllCountryAsync(year);

            var holidaysGroupedByDay = holidays.GroupBy(x => x.Date.DayOfYear).Select(x => new
            {
                dayOfYear = x.Key,
                items = x.ToList()
            }).OrderBy(x => x.items.Count);

            var holiday = holidaysGroupedByDay.First();

            //Since I am sorting by Count, the first item should be a day with 1 country in it. If not, there is no day that meets the desired condition.
            if (holiday.items.Count == 1)
            {
                response.Data = holiday.items.First().CountryCode;
            }

            return response;
        }
    }
}
