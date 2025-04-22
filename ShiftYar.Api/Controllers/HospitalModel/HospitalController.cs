using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShiftYar.Api.Filters;
using ShiftYar.Application.DTOs.HospitalModel;
using ShiftYar.Application.Interfaces.HospitalModel;
using ShiftYar.Infrastructure.Persistence.AppDbContext;

namespace ShiftYar.Api.Controllers.HospitalModel
{
    [Authorize]
    [HasPermission("ManageUsers")]
    [RequireRole("Admin")]
    public class HospitalController : BaseController
    {
        private readonly IHospitalService _hospitalService;

        public HospitalController(ShiftYarDbContext context, IHospitalService hospitalService):base(context)
        {
            _hospitalService = hospitalService;
        }

        //[Authorize]                      //میتونیم در کل کنترلر یا فقط در یک یا چند اکشن مشخص از این فیلتر استفاده کنیم
        //[HasPermission("ManageUsers")]   //میتونیم در کل کنترلر یا فقط در یک یا چند اکشن مشخص از این فیلتر استفاده کنیم
        [HttpGet]
        //[ServiceFilter(typeof(RequestLoggingFilter))]     //میتونیم در کل کنترلر یا فقط در یک یا چند اکشن مشخص از این فیلتر استفاده کنیم
        public async Task<IActionResult> GetHospital()
        {
            var result = await _hospitalService.GetAllHospitalAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddHospital([FromBody] HospitalDto_Add dto)
        {
            await _hospitalService.AddHospitalAsync(dto);
            return Ok();
        }

        //[RequireRole("Admin")]     //میتونیم در کل کنترلر یا فقط در یک یا چند اکشن مشخص از این فیلتر استفاده کنیم
        [HttpDelete("{id}")]
        public IActionResult DeleteHospital(int id)
        {
            // فقط کاربران با نقش Admin این رو می‌تونن بزنن
            return Ok("Hospital deleted.");
        }
    }
}
