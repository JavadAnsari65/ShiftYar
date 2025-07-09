using ShiftYar.Application.Common.Models.ResponseModel;
using ShiftYar.Application.DTOs.ShiftModel.ShiftRequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftYar.Application.Interfaces.ShiftRequestModel
{
    public interface IShiftRequestService
    {
        Task<ApiResponse<ShiftRequestDtoGet>> CreateShiftRequestAsync(ShiftRequestDtoAdd dto);
        Task<ApiResponse<ShiftRequestDtoGet>> UpdateShiftRequestByUserAsync(int id, ShiftRequestDtoAdd dto);
        Task<ApiResponse<ShiftRequestDtoGet>> UpdateShiftRequestBySupervisorAsync(ShiftRequestDtoUpdateBySupervisor dto);
        Task<ApiResponse<string>> DeleteShiftRequestAsync(int id);
        Task<ApiResponse<ShiftRequestDtoGet>> GetShiftRequestAsync(int id);
        Task<ApiResponse<List<ShiftRequestDtoGet>>> GetUserShiftRequestsAsync(int userId);
        Task<ApiResponse<List<ShiftRequestDtoGet>>> GetAllShiftRequestsAsync();
    }
}
