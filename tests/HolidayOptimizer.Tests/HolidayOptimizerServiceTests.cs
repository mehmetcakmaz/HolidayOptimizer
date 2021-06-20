using HolidayOptimizer.API.Model.Domain;
using HolidayOptimizer.API.Services;
using HolidayOptimizer.API.Services.Implementations;
using Moq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
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

            var mockCountryModel = new CountryModel()
            {
                CountryCode = "NL",
                OfficialName = "Netherlands"
            };

            _mockNagerService.Setup(s => s.GetPublicHolidaysForAllCountryAsync(It.IsAny<int>())).ReturnsAsync(() => mockHolidayModel);
            _mockNagerService.Setup(s => s.GetCountryInfoAsync(It.IsAny<string>())).ReturnsAsync(() => mockCountryModel);

            // Act
            var result = await _holidayOptimizerService.GetCountryWithMostHolidaysThisYear();

            // Assert
            Assert.Equal("NL", result.Data.CountryCode);
        }


        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-5)]
        public async void HolidayOptimizerService_GetMonthWithMostHolidaysByYear_Should_ReturnError_When_Year_Not_Valid(int year)
        {
            // Act
            var result = await _holidayOptimizerService.GetMonthWithMostHolidaysByYear(year);

            // Assert
            Assert.True(result.HasError);
            Assert.Equal($"{nameof(year)} parameter must be a valid value.", result.Errors.First());
        }

        [Fact]
        public async void HolidayOptimizerService_GetMonthWithMostHolidaysByYear_Should_ReturnException_When_Year_Is_Valid()
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
            var result = await _holidayOptimizerService.GetMonthWithMostHolidaysByYear(DateTime.Now.Year);

            // Assert
            Assert.False(result.HasError);
            Assert.Equal(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.Month), result.Data.Month);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-5)]
        public async void HolidayOptimizerService_GetCountryWithMostUniqueHolidaysByYear_Should_ReturnError_When_Year_Not_Valid(int year)
        {
            // Act
            var result = await _holidayOptimizerService.GetCountryWithMostUniqueHolidaysByYear(year);

            // Assert
            Assert.True(result.HasError);
            Assert.Equal($"{nameof(year)} parameter must be a valid value.", result.Errors.First());
        }

        [Fact]
        public async void HolidayOptimizerService_GetCountryWithMostUniqueHolidaysByYear_Should_ReturnNotFound_When_Year_Is_Valid()
        {
            // Arrange
            var mockHolidayModel = new List<HolidayModel>()
            {
                new()
                {
                    CountryCode = "NL",
                    Date = Convert.ToDateTime("2021-01-01"),
                    LocalName = "Sample",
                    Name = "Sample"
                },
                new()
                {
                    CountryCode = "DE",
                    Date = Convert.ToDateTime("2021-01-01"),
                    LocalName = "Sample",
                    Name = "Sample"
                },
                new()
                {
                    CountryCode = "TR",
                    Date = Convert.ToDateTime("2021-01-01"),
                    LocalName = "Sample",
                    Name = "Sample"
                }
            };

            _mockNagerService.Setup(s => s.GetPublicHolidaysForAllCountryAsync(It.IsAny<int>())).ReturnsAsync(() => mockHolidayModel);

            // Act
            var result = await _holidayOptimizerService.GetCountryWithMostUniqueHolidaysByYear(DateTime.Now.Year);

            // Assert
            Assert.False(result.HasError);
            Assert.Null(result.Data);
        }

        [Fact]
        public async void HolidayOptimizerService_GetCountryWithMostUniqueHolidaysByYear_Should_Ok_When_Year_Is_Valid()
        {
            // Arrange
            var mockHolidayModel = new List<HolidayModel>()
            {
                new()
                {
                    CountryCode = "NL",
                    Date = Convert.ToDateTime("2021-01-01"),
                    LocalName = "Sample",
                    Name = "Sample"
                },
                new()
                {
                    CountryCode = "NL",
                    Date = Convert.ToDateTime("2021-01-02"),
                    LocalName = "Sample 2",
                    Name = "Sample 1"
                },
                new()
                {
                    CountryCode = "TR",
                    Date = Convert.ToDateTime("2021-01-01"),
                    LocalName = "Sample",
                    Name = "Sample"
                }
            };

            var mockCountryModel = new CountryModel()
            {
                CountryCode = "NL",
                OfficialName = "Netherlands"
            };

            _mockNagerService.Setup(s => s.GetPublicHolidaysForAllCountryAsync(It.IsAny<int>())).ReturnsAsync(() => mockHolidayModel);
            _mockNagerService.Setup(s => s.GetCountryInfoAsync(It.IsAny<string>())).ReturnsAsync(() => mockCountryModel);

            // Act
            var result = await _holidayOptimizerService.GetCountryWithMostUniqueHolidaysByYear(DateTime.Now.Year);

            // Assert
            Assert.False(result.HasError);
            Assert.Equal("NL", result.Data.CountryCode);
        }
    }
}
