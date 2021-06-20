using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HolidayOptimizer.API.Model.Domain;
using HolidayOptimizer.API.Services;
using HolidayOptimizer.API.Services.Implementations;
using Moq;
using Xunit;

namespace HolidayOptimizer.Tests
{
    public class HolidayOptimizerServiceTests
    {
        private readonly HolidayOptimizerService _holidayOptimizerService;
        private Mock<INagerService> _mockNagerService = new Mock<INagerService>();

        public HolidayOptimizerServiceTests()
        {
            _holidayOptimizerService = new HolidayOptimizerService(_mockNagerService.Object);
        }

        [Fact]
        public async Task HolidayOptimizerService_GetCountryWithMostHolidaysThisYear()
        {
            // Arrange
            var mockHolidayModel = new List<HolidayModel>()
            {
                new()
                {
                    CountryCode = "NL",
                    Date = DateTime.Now,
                    LocalName = "Sample",
                    Name = "Sample"
                },
                new()
                {
                    CountryCode = "NL",
                    Date = DateTime.Now,
                    LocalName = "Sample 2",
                    Name = "Sample 1"
                },
                new()
                {
                    CountryCode = "TR",
                    Date = DateTime.Now,
                    LocalName = "Sample",
                    Name = "Sample"
                }
            };

            _mockNagerService.Setup(s => s.GetPublicHolidaysForAllCountryAsync(It.IsAny<int>())).ReturnsAsync(() => mockHolidayModel);

            // Act
            var result = await _holidayOptimizerService.GetCountryWithMostHolidaysThisYear();

            // Assert
            Assert.Equal("NL", result);
        }


        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-5)]
        public void HolidayOptimizerService_GetMonthWithMostHolidaysByYear_Should_ReturnError_When_Year_Not_Valid(int year)
        {
            // Arrange
            

            // Act
            var result = _holidayOptimizerService.GetMonthWithMostHolidaysByYear(year);

            // Assert
            Assert.True(result.HasError);
            Assert.Equal($"{nameof(year)} parameter must be a valid value.", result.Errors.First());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public void HolidayOptimizerService_GetMonthWithMostHolidaysByYear_Should_ReturnException_When_Year_Is_Valid(int year)
        {
            // Arrange
            

            // Act
            var result = _holidayOptimizerService.GetMonthWithMostHolidaysByYear(year);

            // Assert
            Assert.False(result.HasError);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-5)]
        public void HolidayOptimizerService_GetCountryWithMostUniqueHolidaysByYear_Should_ReturnError_When_Year_Not_Valid(int year)
        {
            // Arrange
            

            // Act
            var result = _holidayOptimizerService.GetCountryWithMostUniqueHolidaysByYear(year);

            // Assert
            Assert.True(result.HasError);
            Assert.Equal($"{nameof(year)} parameter must be a valid value.", result.Errors.First());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public void HolidayOptimizerService_GetCountryWithMostUniqueHolidaysByYear_Should_ReturnException_When_Year_Is_Valid(int year)
        {
            // Arrange
            

            // Act
            var result = _holidayOptimizerService.GetCountryWithMostUniqueHolidaysByYear(year);

            // Assert
            Assert.False(result.HasError);
        }
    }
}
