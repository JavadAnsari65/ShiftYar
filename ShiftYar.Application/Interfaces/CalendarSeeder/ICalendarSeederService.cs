using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftYar.Application.Interfaces.CalendarSeeder
{
    public interface ICalendarSeederService
    {
        Task SeedShiftDatesAsync(int year);
    }
}
