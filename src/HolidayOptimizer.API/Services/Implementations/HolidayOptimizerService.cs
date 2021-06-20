using HolidayOptimizer.API.Model.Responses;
using System;
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

        public async Task<string> GetCountryWithMostHolidaysThisYear()
        {
            var result = await _nagerService.GetPublicHolidaysForAllCountryAsync(DateTime.Now.Year);

            var mostHolidayCountry = result.GroupBy(x => x.CountryCode).Select(x => new
            {
                countryCode = x.Key,
                count = x.Count()
            }).OrderByDescending(x => x.count).First();

            return mostHolidayCountry.countryCode;
        }

        public BaseResponse<string> GetMonthWithMostHolidaysByYear(int year)
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

            throw new NotImplementedException("Not implemented");
        }

        public BaseResponse<string> GetCountryWithMostUniqueHolidaysByYear(int year)
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

            throw new NotImplementedException("Not implemented");
        }
    }
}
