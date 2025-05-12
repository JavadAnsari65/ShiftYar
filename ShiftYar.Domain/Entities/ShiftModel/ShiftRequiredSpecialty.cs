using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftYar.Domain.Entities.ShiftModel
{
    ///این مدل مشخص می‌کند در یک شیفت خاص، چه تخصص‌هایی با چه تعداد نیرو لازم هستند
    public class ShiftRequiredSpecialty
    {
        [Key]
        public int? Id { get; set; }

        [ForeignKey("Shift")]
        public int? ShiftId { get; set; }
        public Shift? Shift { get; set; }

        [ForeignKey("Specialty")]
        public int? SpecialtyId { get; set; }
        public Specialty? Specialty { get; set; }

        public int? RequiredMaleCount { get; set; } // اختیاری: تعداد نیروهای مرد
        public int? RequiredFemaleCount { get; set; } // اختیاری: تعداد نیروهای زن
        public int? RequiredAnyGenderCount { get; set; }  //تعداد نیروها صرفنظر از جنسیت

        public int? OnCallMaleCount { get; set; } // اختیاری: تعداد نیروهای مرد آنکال
        public int? OnCallFemaleCount { get; set; } // اختیاری: تعداد نیروهای زن آنکال
        public int? OnCallAnyGenderCount { get; set; }  //تعداد نیروهای آنکال صرفنظر از جنسیت
    }
}
