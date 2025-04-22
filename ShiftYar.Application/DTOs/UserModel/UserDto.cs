using ShiftYar.Domain.Entities.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftYar.Application.DTOs.UserModel
{
    public class UserDto
    {
        //public int? Id { get; set; }

        [Required(ErrorMessage = "نام کامل الزامی است.")]
        public string? FullName { get; set; }

        [Required(ErrorMessage = "شماره تلفن الزامی است.")]
        [Phone(ErrorMessage = "شماره تلفن نامعتبر است.")]
        public string? PhoneNumberMembership { get; set; }

        [Required(ErrorMessage = "کد ملی الزامی است.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "کد ملی باید 10 رقم باشد.")]
        public string? NationalCode { get; set; }

        public string? PersonnelCode { get; set; }

        public DateTime? DateOfEmployment { get; set; }

        public bool? IsProjectPersonnel { get; set; }

        [EmailAddress(ErrorMessage = "ایمیل نامعتبر است.")]
        public string? Email { get; set; }

        public string? Address { get; set; }

        public bool? IsActive { get; set; }

        public string? Image { get; set; }

        //public List<UserPhoneNumber>? OtherPhoneNumbers { get; set; }
        public List<string>? OtherPhoneNumbers { get; set; }

        //public ICollection<UserRole>? UserRoles { get; set; }
        public List<int>? UserRoles { get; set; }
    }

    //public class UserDto
    //{
    //    public int? Id { get; set; }
    //    public string? FullName { get; set; }
    //    public string? PhoneNumberMembership { get; set; }
    //    public string? Password { get; set; }
    //    public string? NationalCode { get; set; }
    //    public string? PersonnelCode { get; set; }
    //    public DateTime? DateOfEmployment { get; set; } // تاریخ استخدام
    //    public bool? IsProjectPersonnel { get; set; } // آیا پرسنل طرحی است یا خیر
    //    public string? Email { get; set; }
    //    public string? Address { get; set; }
    //    public bool? IsActive { get; set; }
    //    public string? Image { get; set; }
    //    public List<UserPhoneNumber>? OtherPhoneNumbers { get; set; }

    //    //هر کاربر می تواند یک یا چند نقش داشته باشد
    //    public ICollection<UserRole>? UserRoles { get; set; } // Admin, User, etc.
    //}
}
