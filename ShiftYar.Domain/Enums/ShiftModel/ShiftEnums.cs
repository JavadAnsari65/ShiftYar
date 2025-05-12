using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftYar.Domain.Enums.ShiftModel
{
    public class ShiftEnums
    {
        public enum ShiftLabel
        {
            Morning = 0,  //شیفت صبح
            Evening = 1,  //شیفت عصر
            Night = 2     //شیفت شب
        }

        public enum ShiftStatus
        {
            Planned = 0,     // برنامه‌ریزی‌شده
            Approved = 0,    // تأیید شده
            Cancelled = 0,   // لغو شده
            Completed = 0    // انجام شده
        }
    }
}
