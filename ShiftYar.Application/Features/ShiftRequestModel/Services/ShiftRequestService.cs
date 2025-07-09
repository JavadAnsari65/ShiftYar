using AutoMapper;
using Microsoft.Extensions.Logging;
using ShiftYar.Application.Common.Models.ResponseModel;
using ShiftYar.Application.DTOs.ShiftModel.ShiftRequestModel;
using ShiftYar.Application.Features.ShiftRequestModel.Filters;
using ShiftYar.Application.Interfaces.Persistence;
using ShiftYar.Application.Interfaces.ShiftRequestModel;
using ShiftYar.Domain.Entities.ShiftRequestModel;
using ShiftYar.Domain.Enums.ShiftRequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftYar.Application.Features.ShiftRequestModel.Services
{
    public class ShiftRequestService : IShiftRequestService
    {
        private readonly IEfRepository<ShiftRequest> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<ShiftRequestService> _logger;

        public ShiftRequestService(IEfRepository<ShiftRequest> repository, IMapper mapper, ILogger<ShiftRequestService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResponse<ShiftRequestDtoGet>> CreateShiftRequestAsync(ShiftRequestDtoAdd dto)
        {
            try
            {
                var entity = _mapper.Map<ShiftRequest>(dto);
                entity.Status = RequestStatus.Pending;
                entity.RequestDate = DateTime.Now;
                await _repository.AddAsync(entity);
                await _repository.SaveAsync();
                var result = _mapper.Map<ShiftRequestDtoGet>(entity);
                return ApiResponse<ShiftRequestDtoGet>.Success(result, "درخواست با موفقیت ثبت شد.");
            }
            catch (Exception ex)
            {
                return ApiResponse<ShiftRequestDtoGet>.Fail($"خطا در ثبت درخواست: {ex.Message}");
            }
        }

        public async Task<ApiResponse<ShiftRequestDtoGet>> UpdateShiftRequestByUserAsync(int id, ShiftRequestDtoAdd dto)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id, "User", "ShiftDate", "Supervisor");
                if (entity == null)
                    return ApiResponse<ShiftRequestDtoGet>.Fail("درخواست مورد نظر یافت نشد.");
                if (entity.Status != RequestStatus.Pending)
                    return ApiResponse<ShiftRequestDtoGet>.Fail("امکان ویرایش این درخواست وجود ندارد.");
                _mapper.Map(dto, entity);
                await _repository.SaveAsync();
                var result = _mapper.Map<ShiftRequestDtoGet>(entity);
                return ApiResponse<ShiftRequestDtoGet>.Success(result, "درخواست با موفقیت ویرایش شد.");
            }
            catch (Exception ex)
            {
                return ApiResponse<ShiftRequestDtoGet>.Fail($"خطا در ویرایش درخواست: {ex.Message}");
            }
        }

        public async Task<ApiResponse<ShiftRequestDtoGet>> UpdateShiftRequestBySupervisorAsync(ShiftRequestDtoUpdateBySupervisor dto)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(dto.Id, "User", "ShiftDate", "Supervisor");
                if (entity == null)
                    return ApiResponse<ShiftRequestDtoGet>.Fail("درخواست مورد نظر یافت نشد.");
                if (entity.Status != RequestStatus.Pending)
                    return ApiResponse<ShiftRequestDtoGet>.Fail("درخواست قبلاً بررسی شده است.");
                entity.Status = dto.Status;
                entity.SupervisorComment = dto.SupervisorComment;
                entity.ApprovalDate = DateTime.Now;
                // SupervisorId باید از context گرفته شود
                await _repository.SaveAsync();
                var result = _mapper.Map<ShiftRequestDtoGet>(entity);
                return ApiResponse<ShiftRequestDtoGet>.Success(result, "درخواست با موفقیت بررسی شد.");
            }
            catch (Exception ex)
            {
                return ApiResponse<ShiftRequestDtoGet>.Fail($"خطا در بررسی درخواست: {ex.Message}");
            }
        }

        public async Task<ApiResponse<string>> DeleteShiftRequestAsync(int id)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                if (entity == null)
                    return ApiResponse<string>.Fail("درخواست مورد نظر یافت نشد.");
                if (entity.Status != RequestStatus.Pending)
                    return ApiResponse<string>.Fail("امکان حذف این درخواست وجود ندارد.");
                _repository.Delete(entity);
                await _repository.SaveAsync();
                return ApiResponse<string>.Success("درخواست با موفقیت حذف شد.");
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.Fail($"خطا در حذف درخواست: {ex.Message}");
            }
        }

        public async Task<ApiResponse<ShiftRequestDtoGet>> GetShiftRequestAsync(int id)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id, "User", "ShiftDate", "Supervisor");
                if (entity == null)
                    return ApiResponse<ShiftRequestDtoGet>.Fail("درخواست مورد نظر یافت نشد.");
                var result = _mapper.Map<ShiftRequestDtoGet>(entity);
                return ApiResponse<ShiftRequestDtoGet>.Success(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<ShiftRequestDtoGet>.Fail($"خطا در دریافت درخواست: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<ShiftRequestDtoGet>>> GetUserShiftRequestsAsync(int userId)
        {
            try
            {
                ShiftRequestFilter filter = new ShiftRequestFilter { UserId = userId };
                (List<ShiftRequest> items, int _) = await _repository.GetByFilterAsync(filter, "User", "ShiftDate", "Supervisor");
                var result = items.Select(x => _mapper.Map<ShiftRequestDtoGet>(x)).ToList();
                return ApiResponse<List<ShiftRequestDtoGet>>.Success(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<ShiftRequestDtoGet>>.Fail($"خطا در دریافت لیست درخواست‌ها: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<ShiftRequestDtoGet>>> GetAllShiftRequestsAsync()
        {
            try
            {
                ShiftRequestFilter filter = new ShiftRequestFilter();
                (List<ShiftRequest> items, int _) = await _repository.GetByFilterAsync(filter, "User", "ShiftDate", "Supervisor");
                var result = items.Select(x => _mapper.Map<ShiftRequestDtoGet>(x)).ToList();
                return ApiResponse<List<ShiftRequestDtoGet>>.Success(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<ShiftRequestDtoGet>>.Fail($"خطا در دریافت لیست درخواست‌ها: {ex.Message}");
            }
        }
    }
}
