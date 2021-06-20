using HolidayOptimizer.API.Model.Responses;
using System;

namespace HolidayOptimizer.API.Services.Implementations
{
    public class HolidayOptimizerService : IHolidayOptimizerService
    {
        public string GetCountryWithMostHolidaysThisYear()
        {
            throw new NotImplementedException("Not implemented.");
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
    }
}
