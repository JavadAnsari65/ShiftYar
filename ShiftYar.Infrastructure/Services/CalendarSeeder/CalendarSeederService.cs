using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using ShiftYar.Application.Interfaces.CalendarSeeder;
using ShiftYar.Domain.Entities.ShiftDateModel;
using ShiftYar.Infrastructure.Persistence.AppDbContext;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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
                // مسیر فایل JSON تعطیلات (مثلاً holidays_1403.json)
                string path = Path.Combine(_env.ContentRootPath, "Resources", "Holidays", $"holidays_{year}.json");

                if (!File.Exists(path))
                    throw new FileNotFoundException($"Holiday file not found for year {year}.");

                // خواندن JSON و تبدیل به لیست تعطیلات
                var json = await File.ReadAllTextAsync(path);
                var holidays = JsonSerializer.Deserialize<List<HolidayEntry>>(json)!;

                var shiftDates = new List<ShiftDate>();
                var persianCalendar = new PersianCalendar();

                // سال 1404/01/01 شمسی رو به معادل میلادی تبدیل کن
                var startDate = persianCalendar.ToDateTime(year, 1, 1, 0, 0, 0, 0);
                var endDate = persianCalendar.ToDateTime(year, 12, 29, 0, 0, 0, 0); // فرض: سال ۱۲ ماه دارد، ماه ۱۲ حداکثر ۲۹ روزه

                // اما بعضی سال‌ها (سال کبیسه)، ماه ۱۲، ۳۰ روزه‌ست
                if (persianCalendar.IsLeapYear(year))
                {
                    endDate = persianCalendar.ToDateTime(year, 12, 30, 0, 0, 0, 0);
                }

                for (var date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    // تاریخ شمسی
                    string persianDate = $"{persianCalendar.GetYear(date):0000}/{persianCalendar.GetMonth(date):00}/{persianCalendar.GetDayOfMonth(date):00}";

                    // بررسی آیا این تاریخ تعطیل است
                    var match = holidays.FirstOrDefault(h => h.Date == persianDate);

                    // نام روز هفته (مثلاً شنبه، یکشنبه و ...)
                    string dayOfWeekTitle = GetPersianDayOfWeekTitle(date.DayOfWeek);
                    // جمعه‌ها رو به‌طور پیش‌فرض تعطیل بدون
                    bool isFriday = date.DayOfWeek == DayOfWeek.Friday;

                    // ایجاد شیفت‌تاریخ جدید
                    var shiftDate = new ShiftDate
                    {
                        Date = date.Date,
                        PersianDate = persianDate,
                        DayTitle = dayOfWeekTitle,
                        IsHoliday = match != null || isFriday,
                        HolidayEvent = match?.Title ?? (date.DayOfWeek == DayOfWeek.Friday ? "تعطیل هفتگی" : null),
                        CreateDate = DateTime.Now,
                        UpdateDate = null,
                        TheUserId = null // یا یک مقدار ثابت در صورت نیاز
                    };

                    shiftDates.Add(shiftDate);
                }

                // ذخیره در دیتابیس
                _context.ShiftDates.AddRange(shiftDates);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("سرویس بروزرسانی تقویم با خطا مواجه شد: " + ex.Message);
            }
        }

        // گرفتن نام فارسی روز هفته
        private string GetPersianDayOfWeekTitle(DayOfWeek dayOfWeek)
        {
            return dayOfWeek switch
            {
                DayOfWeek.Saturday => "شنبه",
                DayOfWeek.Sunday => "یکشنبه",
                DayOfWeek.Monday => "دوشنبه",
                DayOfWeek.Tuesday => "سه‌شنبه",
                DayOfWeek.Wednesday => "چهارشنبه",
                DayOfWeek.Thursday => "پنجشنبه",
                DayOfWeek.Friday => "جمعه",
                _ => "نامشخص"
            };
        }

        public async Task SetAsHolidayAsync(string persianDate, string? holidayEvent)
        {
            try
            {
                // تبدیل تاریخ شمسی به میلادی
                var dateParts = persianDate.Split('/');
                if (dateParts.Length != 3)
                    throw new Exception("فرمت تاریخ شمسی نامعتبر است. فرمت صحیح: yyyy/MM/dd");

                var persianCalendar = new PersianCalendar();
                var year = int.Parse(dateParts[0]);
                var month = int.Parse(dateParts[1]);
                var day = int.Parse(dateParts[2]);

                var date = persianCalendar.ToDateTime(year, month, day, 0, 0, 0, 0);

                var calendarDate = await _context.ShiftDates.FirstOrDefaultAsync(x => x.Date == date.Date);
                if (calendarDate == null)
                {
                    throw new Exception("تاریخ مورد نظر یافت نشد");
                }

                calendarDate.IsHoliday = true;
                calendarDate.HolidayEvent = holidayEvent;
                calendarDate.UpdateDate = DateTime.Now;
                calendarDate.TheUserId = null;

                await _context.SaveChangesAsync();
            }
            catch (FormatException)
            {
                throw new Exception("فرمت تاریخ شمسی نامعتبر است. فرمت صحیح: yyyy/MM/dd");
            }
            catch (Exception ex) when (ex.Message != "تاریخ مورد نظر یافت نشد")
            {
                throw new Exception("خطا در تبدیل تاریخ: " + ex.Message);
            }
        }

        public async Task SetAsRegularDayAsync(string persianDate, string? holidayEvent)
        {
            try
            {
                // تبدیل تاریخ شمسی به میلادی
                var dateParts = persianDate.Split('/');
                if (dateParts.Length != 3)
                    throw new Exception("فرمت تاریخ شمسی نامعتبر است. فرمت صحیح: yyyy/MM/dd");

                var persianCalendar = new PersianCalendar();
                var year = int.Parse(dateParts[0]);
                var month = int.Parse(dateParts[1]);
                var day = int.Parse(dateParts[2]);

                var date = persianCalendar.ToDateTime(year, month, day, 0, 0, 0, 0);

                var calendarDate = await _context.ShiftDates.FirstOrDefaultAsync(x => x.Date == date.Date);
                if (calendarDate == null)
                {
                    throw new Exception("تاریخ مورد نظر یافت نشد");
                }

                calendarDate.IsHoliday = false;
                calendarDate.HolidayEvent = holidayEvent;
                calendarDate.UpdateDate = DateTime.Now;
                calendarDate.TheUserId = null;

                await _context.SaveChangesAsync();
            }
            catch (FormatException)
            {
                throw new Exception("فرمت تاریخ شمسی نامعتبر است. فرمت صحیح: yyyy/MM/dd");
            }
            catch (Exception ex) when (ex.Message != "تاریخ مورد نظر یافت نشد")
            {
                throw new Exception("خطا در تبدیل تاریخ: " + ex.Message);
            }
        }
    }


    // کلاس مدل تعطیلات
    public class HolidayEntry
    {
        public string Date { get; set; } = default!; // yyyy/MM/dd
        public string Title { get; set; } = default!;
    }
}



//using Microsoft.AspNetCore.Hosting;
//using ShiftYar.Application.Interfaces.CalendarSeeder;
//using ShiftYar.Domain.Entities.ShiftDateModel;
//using ShiftYar.Infrastructure.Persistence.AppDbContext;
//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Linq;
//using System.Text;
//using System.Text.Json;
//using System.Threading.Tasks;

//namespace ShiftYar.Infrastructure.Services.CalendarSeeder
//{
//    public class CalendarSeederService : ICalendarSeederService
//    {
//        private readonly ShiftYarDbContext _context;
//        private readonly IWebHostEnvironment _env;

//        public CalendarSeederService(ShiftYarDbContext context, IWebHostEnvironment env)
//        {
//            _context = context;
//            _env = env;
//        }

//        public async Task SeedShiftDatesAsync(int year)
//        {
//            try
//            {
//                string path = Path.Combine(_env.ContentRootPath, "Resources", "Holidays", $"holidays_{year}.json");

//                if (!File.Exists(path))
//                    throw new FileNotFoundException($"Holiday file not found for year {year}.");

//                var json = await File.ReadAllTextAsync(path);
//                var holidays = JsonSerializer.Deserialize<List<HolidayEntry>>(json)!;

//                var startDate = new DateTime(year - 621, 3, 21); // تقریباً ابتدای سال شمسی
//                var endDate = startDate.AddYears(1);

//                var shiftDates = new List<ShiftDate>();

//                for (var date = startDate; date < endDate; date = date.AddDays(1))
//                {
//                    var persian = new PersianCalendar();
//                    string persianDate = $"{persian.GetYear(date):0000}/{persian.GetMonth(date):00}/{persian.GetDayOfMonth(date):00}";

//                    var match = holidays.FirstOrDefault(h => h.Date == 
//                    );

//                    shiftDates.Add(new ShiftDate
//                    {
//                        Date = date.Date,
//                        PersianDate = persianDate,
//                        IsHoliday = match != null,
//                        HolidayEvent = match?.Title
//                    });
//                }

//                _context.ShiftDates.AddRange(shiftDates);
//                await _context.SaveChangesAsync();
//            }
//            catch (Exception ex)
//            {
//                throw new Exception("سرویس بروزرسانی تقویم با خطا مواجه شد : " + ex.Message);
//            }
//        }
//    }

//    public class HolidayEntry
//    {
//        public string Date { get; set; } = default!; // yyyy/MM/dd
//        public string Title { get; set; } = default!;
//    }
//}
