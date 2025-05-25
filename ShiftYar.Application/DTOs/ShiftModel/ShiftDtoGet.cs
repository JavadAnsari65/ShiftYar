using ShiftYar.Domain.Entities.DepartmentModel;
using ShiftYar.Domain.Entities.ShiftModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ShiftYar.Domain.Enums.ShiftModel.ShiftEnums;

namespace ShiftYar.Application.DTOs.ShiftModel
{
    public class ShiftDtoGet
    {
        public int? Id { get; set; }

        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }

        public ShiftLabel? Label { get; set; } // صبح، عصر، شب

        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }

        //public ICollection<ShiftRequiredSpecialty>? RequiredSpecialties { get; set; }
    }
}
