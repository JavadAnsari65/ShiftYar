using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShiftYar.Application.Interfaces.CalendarSeeder;
using ShiftYar.Infrastructure.Persistence.AppDbContext;

namespace ShiftYar.Api.Controllers.CalendarSeeder
{
    public class CalendarSeederController : BaseController
    {
        private readonly ICalendarSeederService _calendarSeeder;
        public CalendarSeederController(ShiftYarDbContext context, ICalendarSeederService calendarSeeder) : base(context)
        {
            _calendarSeeder = calendarSeeder;
        }

        [HttpPost]
        public async Task<IActionResult> SeedYear(int year)
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

        [HttpPut]
        public async Task<IActionResult> SetDayAsHoliday(string persianDate, string? holidayEvent)
        {
            try
            {
                await _calendarSeeder.SetAsHolidayAsync(persianDate, holidayEvent);
                return Ok(new { message = "تاریخ موردنظر با موفقیت به عنوان روز تعطیل ثبت شد" });
            }
            catch (Exception ex)
            {
                return BadRequest("عملیات با خطا مواجه شد : " + ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> SetDayAsRegularDay(string persianDate, string? holidayEvent)
        {
            try
            {
                await _calendarSeeder.SetAsRegularDayAsync(persianDate, holidayEvent);
                return Ok(new { message = "تاریخ موردنظر با موفقیت به عنوان روز عادی ثبت شد" });
            }
            catch (Exception ex)
            {
                return BadRequest("عملیات با خطا مواجه شد : " + ex.Message);
            }
        }
    }
}
