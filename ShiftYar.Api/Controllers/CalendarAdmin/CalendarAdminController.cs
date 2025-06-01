using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShiftYar.Application.Interfaces.CalendarSeeder;
using ShiftYar.Infrastructure.Persistence.AppDbContext;

namespace ShiftYar.Api.Controllers.CalendarAdmin
{
    public class CalendarAdminController : BaseController
    {
        private readonly ICalendarSeederService _calendarSeeder;
        public CalendarAdminController(ShiftYarDbContext context, ICalendarSeederService calendarSeeder) : base(context)
        {
            _calendarSeeder = calendarSeeder;
        }

        [HttpPost]
        public async Task<IActionResult> SeedYear([FromRoute] int year)
        {
            try
            {
                await _calendarSeeder.SeedShiftDatesAsync(year);
                return Ok(new { message = $"Calendar for {year} seeded successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest("عملیات بروزرسانی تقویم با خطا مواجه شد : " + ex.Message);
            }
        }
    }
}
