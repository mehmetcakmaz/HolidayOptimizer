using HolidayOptimizer.API.Model.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HolidayOptimizer.API.Services
{
    public interface INagerService
    {
        Task<IEnumerable<HolidayModel>> GetPublicHolidayAsync(string countryCode, int year);
        Task<IEnumerable<HolidayModel>> GetPublicHolidaysForAllCountryAsync(int year);
    }
}