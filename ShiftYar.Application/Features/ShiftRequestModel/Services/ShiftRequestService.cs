using AutoMapper;
using Microsoft.Extensions.Logging;
using ShiftYar.Application.Common.Models.ResponseModel;
using ShiftYar.Application.DTOs.ShiftModel.ShiftRequestModel;
using ShiftYar.Application.Features.ShiftRequestModel.Filters;
using ShiftYar.Application.Interfaces.Persistence;
using ShiftYar.Application.Interfaces.ShiftRequestModel;
using ShiftYar.Domain.Entities.DepartmentModel;
using ShiftYar.Domain.Entities.ShiftRequestModel;
using ShiftYar.Domain.Entities.UserModel;
using ShiftYar.Domain.Enums.ShiftRequestModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftYar.Application.Features.ShiftRequestModel.Services
{
    public class ShiftRequestService : IShiftRequestService
    {
        private readonly IEfRepository<ShiftRequest> _repository;
        private readonly IEfRepository<User> _repositoryUser;
        private readonly IEfRepository<Department> _repositorDepartment;
        private readonly IMapper _mapper;
        private readonly ILogger<ShiftRequestService> _logger;

        public ShiftRequestService(IEfRepository<ShiftRequest> repository, IEfRepository<User> repositoryUser, IEfRepository<Department> repositorDepartment, IMapper mapper, ILogger<ShiftRequestService> logger)
        {
            _repository = repository;
            _repositoryUser = repositoryUser;
            _repositorDepartment = repositorDepartment;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResponse<ShiftRequestDtoGet>> CreateShiftRequestAsync(ShiftRequestDtoAdd dto)
        {
            try
            {
                //استخراج دپارتمان کاربر، برای دسترسی به سوپروایزر دپارتمان
                var userDepatmentId = _repositoryUser.GetByIdAsync(dto.UserId).Result.DepartmentId;

                if(!userDepatmentId.HasValue)
                    return ApiResponse<ShiftRequestDtoGet>.Fail("کاربر درخواست دهنده، متعلق به هیچ دپارتمانی نیست.");

                //استخراج شناسه سوپروایزر
                var supervisorId = _repositorDepartment.GetByIdAsync(userDepatmentId).Result.SupervisorId;

                if(!supervisorId.HasValue)
                    return ApiResponse<ShiftRequestDtoGet>.Fail("fبرای دپارتمان کاربر درخواست دهنده، سوپروایزر تعیین نشده است.");

                var entity = _mapper.Map<ShiftRequest>(dto);
                entity.RequestDate = ConvertToGregorianDate(dto.RequestPersianDate);
                entity.SupervisorId = supervisorId;
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
                var entity = await _repository.GetByIdAsync(dto.Id, "User", "Supervisor");
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
                var entity = await _repository.GetByIdAsync(id, "User", "Supervisor");
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
                (List<ShiftRequest> items, int _) = await _repository.GetByFilterAsync(filter, "User", "Supervisor");
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
                (List<ShiftRequest> items, int _) = await _repository.GetByFilterAsync(filter, "User", "Supervisor");
                var result = items.Select(x => _mapper.Map<ShiftRequestDtoGet>(x)).ToList();
                return ApiResponse<List<ShiftRequestDtoGet>>.Success(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<ShiftRequestDtoGet>>.Fail($"خطا در دریافت لیست درخواست‌ها: {ex.Message}");
            }
        }


        //متد خصوصی برای تبدیل رشته تاریخ شمسی به تاریخ میلادی
        private DateTime ConvertToGregorianDate(string persianDate)
        {
            if (string.IsNullOrWhiteSpace(persianDate))
                throw new ArgumentException("تاریخ وارد شده نامعتبر است");

            // رشته تاریخ را به بخش‌های سال، ماه و روز تقسیم می‌کنیم
            var parts = persianDate.Split('/');
            if (parts.Length != 3)
                throw new FormatException("فرمت تاریخ وارد شده صحیح نیست");

            int year = int.Parse(parts[0]);
            int month = int.Parse(parts[1]);
            int day = int.Parse(parts[2]);

            var persianCalendar = new PersianCalendar();
            return persianCalendar.ToDateTime(year, month, day, 0, 0, 0, 0);
        }
    }
}
