using HolidayOptimizer.API.Model.Responses;
using System.Threading.Tasks;

namespace HolidayOptimizer.API.Services
{
    public interface IHolidayOptimizerService
    {
        Task<BaseResponse<GetCountryWithMostHolidaysResponse>> GetCountryWithMostHolidaysThisYear();
        Task<BaseResponse<GetMonthWithMostHolidaysResponse>> GetMonthWithMostHolidaysByYear(int year);
        Task<BaseResponse<GetCountryWithMostUniqueHolidaysResponse>> GetCountryWithMostUniqueHolidaysByYear(int year);
    }
}