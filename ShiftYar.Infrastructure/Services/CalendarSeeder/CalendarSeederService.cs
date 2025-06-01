using Microsoft.AspNetCore.Hosting;
using ShiftYar.Application.Interfaces.CalendarSeeder;
using ShiftYar.Domain.Entities.ShiftDateModel;
using ShiftYar.Infrastructure.Persistence.AppDbContext;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShiftYar.Infrastructure.Services.CalendarSeeder
{
    public class CalendarSeederService : ICalendarSeederService
    {
        private readonly ShiftYarDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CalendarSeederService(ShiftYarDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task SeedShiftDatesAsync(int year)
        {
            try
            {
                string path = Path.Combine(_env.ContentRootPath, "Resources", "Holidays", $"holidays_{year}.json");

                if (!File.Exists(path))
                    throw new FileNotFoundException($"Holiday file not found for year {year}.");

                var json = await File.ReadAllTextAsync(path);
                var holidays = JsonSerializer.Deserialize<List<HolidayEntry>>(json)!;

                var startDate = new DateTime(year - 621, 3, 21); // تقریباً ابتدای سال شمسی
                var endDate = startDate.AddYears(1);

                var shiftDates = new List<ShiftDate>();

                for (var date = startDate; date < endDate; date = date.AddDays(1))
                {
                    var persian = new PersianCalendar();
                    string persianDate = $"{persian.GetYear(date):0000}/{persian.GetMonth(date):00}/{persian.GetDayOfMonth(date):00}";

                    var match = holidays.FirstOrDefault(h => h.Date == persianDate);

                    shiftDates.Add(new ShiftDate
                    {
                        Date = date.Date,
                        PersianDate = persianDate,
                        IsHoliday = match != null,
                        HolidayTitle = match?.Title
                    });
                }

                _context.ShiftDates.AddRange(shiftDates);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("سرویس بروزرسانی تقویم با خطا مواجه شد : " + ex.Message);
            }
        }
    }

    public class HolidayEntry
    {
        public string Date { get; set; } = default!; // yyyy/MM/dd
        public string Title { get; set; } = default!;
    }
}
