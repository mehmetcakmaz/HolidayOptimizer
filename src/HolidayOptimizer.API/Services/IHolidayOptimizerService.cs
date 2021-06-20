using HolidayOptimizer.API.Model.Responses;
using System.Threading.Tasks;

namespace HolidayOptimizer.API.Services
{
    public interface IHolidayOptimizerService
    {
        Task<BaseResponse<GetCountryWithMostHolidaysResponse>> GetCountryWithMostHolidaysThisYear();
        Task<BaseResponse<string>> GetMonthWithMostHolidaysByYear(int year);
        Task<BaseResponse<string>> GetCountryWithMostUniqueHolidaysByYear(int year);
    }
}