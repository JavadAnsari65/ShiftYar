using ShiftYar.Application.Common.Models.ResponseModel;
using ShiftYar.Application.DTOs.UserModel;
using ShiftYar.Application.Features.UserModel.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftYar.Application.Interfaces.UserModel
{
    public interface IUserService
    {
        Task<ApiResponse<List<UserDto>>> GetFilteredUsersAsync(UserFilter filter);
        Task<ApiResponse<UserDto>> GetByIdAsync(int id);
        Task<ApiResponse<UserDto>> CreateAsync(UserDto dto);
        Task<ApiResponse<UserDto>> UpdateAsync(int id, UserDto dto);
        Task<ApiResponse<string>> DeleteAsync(int id);
    }
}
