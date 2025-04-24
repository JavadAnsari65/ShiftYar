using AutoMapper;
using Microsoft.Extensions.Logging;
using ShiftYar.Application.Common.Models.ResponseModel;
using ShiftYar.Application.DTOs.UserModel;
using ShiftYar.Application.Features.UserModel.Filters;
using ShiftYar.Application.Interfaces.Persistence;
using ShiftYar.Application.Interfaces.UserModel;
using ShiftYar.Domain.Entities.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftYar.Application.Features.UserModel.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _repository;
        private readonly IMapper _mapper; // AutoMapper

        public UserService(IRepository<User> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<UserDtoGet>>> GetFilteredUsersAsync(UserFilter filter)
        {
            // includeAllNestedCollections By Default true
            //var users = await _repository.GetByFilterAsync(filter);
            var users = await _repository.GetByFilterAsync(filter, includeAllNestedCollections: true);
            // var users = await _repository.GetByFilterAsync(filter, includeAllNestedCollections: false);
            var data = _mapper.Map<List<UserDtoGet>>(users);
            return ApiResponse<List<UserDtoGet>>.Success(data);
        }

        public async Task<ApiResponse<UserDtoAdd>> GetByIdAsync(int id)
        {
            var user = await _repository.GetByIdAsync(id);
            if (user == null) return ApiResponse<UserDtoAdd>.Fail("کاربر یافت نشد.");

            var dto = _mapper.Map<UserDtoAdd>(user);
            return ApiResponse<UserDtoAdd>.Success(dto);
        }

        public async Task<ApiResponse<UserDtoAdd>> CreateAsync(UserDtoAdd dto)
        {
            if (!string.IsNullOrEmpty(dto.NationalCode))
            {
                var exists = await _repository.ExistsAsync(u => u.NationalCode == dto.NationalCode);
                if (exists) return ApiResponse<UserDtoAdd>.Fail("کاربری با این کد ملی وجود دارد.");
            }

            var user = _mapper.Map<User>(dto);
            await _repository.AddAsync(user);
            await _repository.SaveAsync();

            var result = _mapper.Map<UserDtoAdd>(user);
            return ApiResponse<UserDtoAdd>.Success(result, "کاربر با موفقیت ایجاد شد.");
        }

        public async Task<ApiResponse<UserDtoAdd>> UpdateAsync(int id, UserDtoAdd dto)
        {
            var user = await _repository.GetByIdAsync(id);
            if (user == null) return ApiResponse<UserDtoAdd>.Fail("کاربر یافت نشد.");

            _mapper.Map(dto, user);
            _repository.Update(user);
            await _repository.SaveAsync();

            var result = _mapper.Map<UserDtoAdd>(user);
            return ApiResponse<UserDtoAdd>.Success(result, "ویرایش انجام شد.");
        }

        public async Task<ApiResponse<string>> DeleteAsync(int id)
        {
            var user = await _repository.GetByIdAsync(id);
            if (user == null) return ApiResponse<string>.Fail("کاربر یافت نشد.");

            _repository.Delete(user);
            await _repository.SaveAsync();

            return ApiResponse<string>.Success("کاربر حذف شد.");
        }
    }

}



//using AutoMapper;
//using Microsoft.Extensions.Logging;
//using ShiftYar.Application.Common.Models.ResponseModel;
//using ShiftYar.Application.DTOs.UserModel;
//using ShiftYar.Application.Features.UserModel.Filters;
//using ShiftYar.Application.Interfaces.Persistence;
//using ShiftYar.Application.Interfaces.UserModel;
//using ShiftYar.Domain.Entities.UserModel;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ShiftYar.Application.Features.UserModel.Services
//{
//    public class UserService : IUserService
//    {
//        private readonly IRepository<User> _repository;
//        private readonly IMapper _mapper; // AutoMapper

//        public UserService(IRepository<User> repository, IMapper mapper)
//        {
//            _repository = repository;
//            _mapper = mapper;
//        }

//        public async Task<ApiResponse<List<UserDto>>> GetFilteredUsersAsync(UserFilter filter)
//        {
//            var users = await _repository.GetByFilterAsync(filter);
//            var data = _mapper.Map<List<UserDto>>(users);
//            return ApiResponse<List<UserDto>>.Success(data);
//        }

//        public async Task<ApiResponse<UserDto>> GetByIdAsync(int id)
//        {
//            var user = await _repository.GetByIdAsync(id);
//            if (user == null) return ApiResponse<UserDto>.Fail("کاربر یافت نشد.");

//            var dto = _mapper.Map<UserDto>(user);
//            return ApiResponse<UserDto>.Success(dto);
//        }

//        public async Task<ApiResponse<UserDto>> CreateAsync(UserDto dto)
//        {
//            if (!string.IsNullOrEmpty(dto.NationalCode))
//            {
//                var exists = await _repository.ExistsAsync(u => u.NationalCode == dto.NationalCode);
//                if (exists) return ApiResponse<UserDto>.Fail("کاربری با این کد ملی وجود دارد.");
//            }

//            var user = _mapper.Map<User>(dto);
//            await _repository.AddAsync(user);
//            await _repository.SaveAsync();

//            var result = _mapper.Map<UserDto>(user);
//            return ApiResponse<UserDto>.Success(result, "کاربر با موفقیت ایجاد شد.");
//        }

//        public async Task<ApiResponse<UserDto>> UpdateAsync(int id, UserDto dto)
//        {
//            var user = await _repository.GetByIdAsync(id);
//            if (user == null) return ApiResponse<UserDto>.Fail("کاربر یافت نشد.");

//            _mapper.Map(dto, user);
//            _repository.Update(user);
//            await _repository.SaveAsync();

//            var result = _mapper.Map<UserDto>(user);
//            return ApiResponse<UserDto>.Success(result, "ویرایش انجام شد.");
//        }

//        public async Task<ApiResponse<string>> DeleteAsync(int id)
//        {
//            var user = await _repository.GetByIdAsync(id);
//            if (user == null) return ApiResponse<string>.Fail("کاربر یافت نشد.");

//            _repository.Delete(user);
//            await _repository.SaveAsync();

//            return ApiResponse<string>.Success("کاربر حذف شد.");
//        }
//    }

//}
