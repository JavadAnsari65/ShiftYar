using ShiftYar.Application.Common.Filters;
using ShiftYar.Domain.Entities.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShiftYar.Application.Features.UserModel.Filters
{
    //فیلتر کاربر
    public class UserFilter : BaseFilter<User>
    {
        public int? Id { get; set; } // شناسه کاربر
        public string FullName { get; set; } // نام کامل
        public string NationalCode { get; set; } // کد ملی
        public string PhoneNumberMembership { get; set; } // شماره تلفن عضویت
        public bool? IsActive { get; set; } // وضعیت فعال بودن
        public bool? IsProjectPersonnel { get; set; } // پرسنل طرحی بودن
        public int? DepartmentId { get; set; } // شناسه دپارتمان
        public string Search { get; set; } // 🔍 جستجوی ترکیبی

        // Pagination parameters
        public int PageNumber { get; set; } = 1; // شماره صفحه پیش‌فرض: 1
        public int PageSize { get; set; } = 10; // تعداد رکورد در هر صفحه پیش‌فرض: 10

        public override Expression<Func<User, bool>> GetExpression()
        {
            Expression<Func<User, bool>> expression = user => true; // مقدار اولیه بدون شرط

            // فیلتر براساس شناسه
            if (Id.HasValue)
            {
                Expression<Func<User, bool>> idExpr = user => user.Id == Id;
                expression = CombineExpressions(expression, idExpr);
            }

            // فیلتر براساس نام کامل
            if (!string.IsNullOrEmpty(FullName))
            {
                Expression<Func<User, bool>> fullNameExpr = user => user.FullName.Contains(FullName);
                expression = CombineExpressions(expression, fullNameExpr);
            }

            // فیلتر براساس کد ملی
            if (!string.IsNullOrEmpty(NationalCode))
            {
                Expression<Func<User, bool>> nationalCodeExpr = user => user.NationalCode == NationalCode;
                expression = CombineExpressions(expression, nationalCodeExpr);
            }

            // فیلتر براساس شماره عضویت
            if (!string.IsNullOrEmpty(PhoneNumberMembership))
            {
                Expression<Func<User, bool>> phoneExpr = user => user.PhoneNumberMembership == PhoneNumberMembership;
                expression = CombineExpressions(expression, phoneExpr);
            }

            // فیلتر براساس فعال بودن
            if (IsActive.HasValue)
            {
                Expression<Func<User, bool>> isActiveExpr = user => user.IsActive == IsActive;
                expression = CombineExpressions(expression, isActiveExpr);
            }

            // فیلتر براساس پرسنل طرحی بودن
            if (IsProjectPersonnel.HasValue)
            {
                Expression<Func<User, bool>> isProjectExpr = user => user.IsProjectPersonnel == IsProjectPersonnel;
                expression = CombineExpressions(expression, isProjectExpr);
            }

            // فیلتر براساس شناسه دپارتمان
            if (DepartmentId.HasValue)
            {
                Expression<Func<User, bool>> departmentExpr = user => user.DepartmentId == DepartmentId;
                expression = CombineExpressions(expression, departmentExpr);
            }

            // جستجوی ترکیبی
            if (!string.IsNullOrEmpty(Search))
            {
                Expression<Func<User, bool>> searchExpr = user =>
                    user.FullName != null && user.FullName.Contains(Search) ||
                    user.NationalCode != null && user.NationalCode.Contains(Search) ||
                    user.PhoneNumberMembership != null && user.PhoneNumberMembership.Contains(Search) ||
                    user.Email != null && user.Email.Contains(Search);

                expression = CombineExpressions(expression, searchExpr);
            }

            return expression;
        }
    }

}
