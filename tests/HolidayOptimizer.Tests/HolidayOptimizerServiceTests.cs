using HolidayOptimizer.API.Services.Implementations;
using Xunit;

namespace HolidayOptimizer.Tests
{
    public class HolidayOptimizerServiceTests
    {
        [Fact]
        public void HolidayOptimizerService_GetCountryWithMostHolidaysThisYear()
        {
            // Arrange
            var holidayOptimizerService = new HolidayOptimizerService();

            // Act
            var result = holidayOptimizerService.GetCountryWithMostHolidaysThisYear();

            // Assert
            Assert.NotEmpty(result);
        }


        [Fact]
        public void HolidayOptimizerService_GetMonthWithMostHolidaysByYear()
        {
            // Arrange
            var holidayOptimizerService = new HolidayOptimizerService();

            // Act
            var result = holidayOptimizerService.GetCountryWithMostHolidaysThisYear();

            // Assert
            Assert.NotEmpty(result);
        }
    }
}
