using HolidayOptimizer.API.Services.Implementations;
using Xunit;

namespace HolidayOptimizer.Tests
{
    public class HolidayOptimizerServiceTests
    {
        const string MOST_HOLIDAY_COUNTRY_SAMPLE = "NL";

        [Fact]
        public void HolidayOptimizerService_GetCountryWithMostHolidaysThisYear()
        {
            // Arrange
            var holidayOptimizerService = new HolidayOptimizerService();

            // Act
            var result = holidayOptimizerService.GetCountryWithMostHolidaysThisYear();

            // Assert
            Assert.Equal(MOST_HOLIDAY_COUNTRY_SAMPLE, result);
        }
    }
}
