using ShiftYar.Domain.Entities.DepartmentModel;
using ShiftYar.Domain.Entities.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftYar.Application.DTOs.UserModel
{
    public class UserDtoGet
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumberMembership { get; set; }
        public string? NationalCode { get; set; }
        public string? PersonnelCode { get; set; }
        public DateTime? DateOfEmployment { get; set; }
        public bool? IsProjectPersonnel { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public bool? IsActive { get; set; }
        public string? Image { get; set; }
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }

        public List<UserPhoneNumber>? OtherPhoneNumbers { get; set; }
        public List<UserRole>? UserRoles { get; set; }
    }
}
