using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShiftYar.Api.Controllers.UserModel;
using ShiftYar.Api.Filters;
using ShiftYar.Application.DTOs.HospitalModel;
using ShiftYar.Application.Features.HospitalModel.Filters;
using ShiftYar.Application.Features.UserModel.Filters;
using ShiftYar.Application.Interfaces.HospitalModel;
using ShiftYar.Infrastructure.Persistence.AppDbContext;

namespace ShiftYar.Api.Controllers.HospitalModel
{
    //[Authorize]
    //[HasPermission("ManageUsers")]
    //[RequireRole("Admin")]
    public class HospitalController : BaseController
    {
        private readonly IHospitalService _hospitalService;
        private readonly ILogger<HospitalController> _logger;

        public HospitalController(ShiftYarDbContext context, IHospitalService hospitalService, ILogger<HospitalController> logger) : base(context)
        {
            _hospitalService = hospitalService;
            _logger = logger;
        }

        //دریافت لیست بیمارستان ها
        //[Authorize]                      //میتونیم در کل کنترلر یا فقط در یک یا چند اکشن مشخص از این فیلتر استفاده کنیم
        //[HasPermission("ManageUsers")]   //میتونیم در کل کنترلر یا فقط در یک یا چند اکشن مشخص از این فیلتر استفاده کنیم
        [HttpGet]
        //[ServiceFilter(typeof(RequestLoggingFilter))]     //میتونیم در کل کنترلر یا فقط در یک یا چند اکشن مشخص از این فیلتر استفاده کنیم
        public async Task<IActionResult> GetHospitals([FromQuery] HospitalFilter filter)
        {
            var result = await _hospitalService.GetFilteredUsersAsync(filter);
            return Ok(result);
        }


    }
}
