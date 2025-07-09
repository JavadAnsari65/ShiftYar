using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShiftYar.Application.DTOs.ShiftModel.ShiftRequestModel;
using ShiftYar.Application.Interfaces.ShiftRequestModel;
using ShiftYar.Infrastructure.Persistence.AppDbContext;

namespace ShiftYar.Api.Controllers.ShiftRequestModel
{
    public class ShiftRequestController : BaseController
    {
        private readonly IShiftRequestService _service;
        public ShiftRequestController(ShiftYarDbContext context, IShiftRequestService service) : base(context)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateShiftRequest([FromBody] ShiftRequestDtoAdd dto)
        {
            var result = await _service.CreateShiftRequestAsync(dto);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateShiftRequestByUser(int id, [FromBody] ShiftRequestDtoAdd dto)
        {
            var result = await _service.UpdateShiftRequestByUserAsync(id, dto);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateShiftRequestBySupervisor([FromBody] ShiftRequestDtoUpdateBySupervisor dto)
        {
            var result = await _service.UpdateShiftRequestBySupervisorAsync(dto);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteShiftRequest(int id)
        {
            var result = await _service.DeleteShiftRequestAsync(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetShiftRequest(int id)
        {
            var result = await _service.GetShiftRequestAsync(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserShiftRequests(int userId)
        {
            var result = await _service.GetUserShiftRequestsAsync(userId);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllShiftRequest()
        {
            var result = await _service.GetAllShiftRequestsAsync();
            return Ok(result);
        }
    }
}
