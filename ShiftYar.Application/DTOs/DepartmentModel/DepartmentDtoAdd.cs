﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftYar.Application.DTOs.DepartmentModel
{
    public class DepartmentDtoAdd
    {
        [Required(ErrorMessage = "نام دپارتمان الزامی است.")]
        public string? Name { get; set; }

        public string? Description { get; set; }

        public bool? IsActive { get; set; }

        [Required(ErrorMessage = "شناسه بیمارستان الزامی است.")]
        public int? HospitalId { get; set; }
    }
}