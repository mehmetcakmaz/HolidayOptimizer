using System.Linq;
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


        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-5)]
        public void HolidayOptimizerService_GetMonthWithMostHolidaysByYear_Should_ReturnError_When_Year_Not_Valid(int year)
        {
            // Arrange
            var holidayOptimizerService = new HolidayOptimizerService();

            // Act
            var result = holidayOptimizerService.GetMonthWithMostHolidaysByYear(year);

            // Assert
            Assert.True(result.HasError);
            Assert.Equal($"{nameof(year)} parameter must be a valid value.", result.Errors.First());
        }
    }
}
