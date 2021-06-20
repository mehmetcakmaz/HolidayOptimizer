using System.Collections.Generic;
using HolidayOptimizer.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using HolidayOptimizer.API.Model.Responses;

namespace HolidayOptimizer.API.Controllers
{
    [Route("api/holidays/")]
    [ApiController]
    public class HolidaysController : ControllerBase
    {
        private readonly IHolidayOptimizerService _holidayOptimizerService;

        public HolidaysController(IHolidayOptimizerService holidayOptimizerService)
        {
            _holidayOptimizerService = holidayOptimizerService;
        }

        [HttpGet]
        [Route("countrywithmostholidaysthisyear")]
        [ProducesResponseType(typeof(GetCountryWithMostHolidaysResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountryWithMostHolidaysThisYear()
        {
            var response = await _holidayOptimizerService.GetCountryWithMostHolidaysThisYear();

            return Ok(response.Data);
        }

        [HttpGet]
        [Route("monthwithmostholidays/{year}")]
        [ProducesResponseType(typeof(GetMonthWithMostHolidaysResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetMonthWithMostHolidaysByYear([FromRoute] int year)
        {
            var response = await _holidayOptimizerService.GetMonthWithMostHolidaysByYear(year);

            if (response.HasError)
            {
                return BadRequest(response.Errors);
            }

            return Ok(response.Data);
        }

        [HttpGet]
        [Route("countrywithmostuniqueholidays/{year}")]
        [ProducesResponseType(typeof(GetCountryWithMostUniqueHolidaysResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCountryWithMostUniqueHolidaysByYear([FromRoute] int year)
        {
            var response = await _holidayOptimizerService.GetCountryWithMostUniqueHolidaysByYear(year);

            if (response.HasError)
            {
                return BadRequest(response.Errors);
            }

            if (response.Data == null)
            {
                return NotFound();
            }

            return Ok(response.Data);
        }
    }
}
