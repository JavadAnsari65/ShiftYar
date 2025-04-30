using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShiftYar.Application.Common.Models.ResponseModel;
using ShiftYar.Application.DTOs.RoleModel;
using ShiftYar.Application.Features.RoleModel.Filters;
using ShiftYar.Application.Interfaces.RoleModel;
using ShiftYar.Infrastructure.Persistence.AppDbContext;

namespace ShiftYar.Api.Controllers.RoleModel
{
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;

        public RoleController(ShiftYarDbContext context, IRoleService roleService) : base(context)
        {
            _roleService = roleService;
        }

        ///Get All Roles
        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResponse<RoleDtoGet>>>> GetRoles([FromQuery] RoleFilter filter)
        {
            var result = await _roleService.GetFilteredRolesAsync(filter);
            return Ok(result);
        }

        ///Get Role By Id
        [HttpGet]
        public async Task<ActionResult<ApiResponse<RoleDtoGet>>> GetRole(int id)
        {
            var result = await _roleService.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        ///Create Role
        [HttpPost]
        public async Task<ActionResult<ApiResponse<RoleDtoGet>>> CreateRole([FromBody] RoleDtoAdd dto)
        {
            var result = await _roleService.CreateAsync(dto);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return CreatedAtAction(nameof(GetRole), new { id = result.Data.Id }, result);
        }

        ///Update Role
        [HttpPut]
        public async Task<ActionResult<ApiResponse<RoleDtoGet>>> UpdateRole(int id, [FromBody] RoleDtoAdd dto)
        {
            var result = await _roleService.UpdateAsync(id, dto);
            if (!result.IsSuccess)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        ///Delete Role
        [HttpDelete]
        public async Task<ActionResult<ApiResponse<string>>> DeleteRole(int id)
        {
            var result = await _roleService.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                return NotFound(result);
            }
            return Ok(result);
        }
    }
}