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
        private readonly IEfRepository<User> _repository;
        private readonly IEfRepository<UserPhoneNumber> _repositoryPhoneNumber;
        private readonly IEfRepository<UserRole> _repositoryUserRole;
        private readonly IMapper _mapper; // AutoMapper

        public UserService(IEfRepository<User> repository, IEfRepository<UserPhoneNumber> repositoryPhoneNumber, 
                                IEfRepository<UserRole> repositoryUserRole, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _repositoryPhoneNumber = repositoryPhoneNumber;
            _repositoryUserRole = repositoryUserRole;
        }

        public async Task<ApiResponse<List<UserDtoGet>>> GetFilteredUsersAsync(UserFilter filter)
        {
            var users = await _repository.GetByFilterAsync(
                filter,
                "OtherPhoneNumbers",
                "UserRoles",
                "UserRoles.Role"
            );
            var data = _mapper.Map<List<UserDtoGet>>(users);
            return ApiResponse<List<UserDtoGet>>.Success(data);
        }

        
        ///Get User
        public async Task<ApiResponse<UserDtoAdd>> GetByIdAsync(int id)
        {
            var user = await _repository.GetByIdAsync(id, "OtherPhoneNumbers", "UserRoles", "UserRoles.Role");
            if (user == null) return ApiResponse<UserDtoAdd>.Fail("کاربر یافت نشد.");

            var dto = _mapper.Map<UserDtoAdd>(user);
            return ApiResponse<UserDtoAdd>.Success(dto);
        }


        ///Create User
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


        ///Update User
        public async Task<ApiResponse<UserDtoAdd>> UpdateAsync(int id, UserDtoAdd dto)
        {
            var user = await _repository.GetByIdAsync(id, "OtherPhoneNumbers", "UserRoles", "UserRoles.Role");
            if (user == null) return ApiResponse<UserDtoAdd>.Fail("کاربر یافت نشد.");

            // Handle phone numbers
            if (dto.OtherPhoneNumbers != null)
            {
                // If the list is empty, remove all phone numbers
                if (dto.OtherPhoneNumbers.Count == 0)
                {
                    foreach (var phoneNumber in user.OtherPhoneNumbers.ToList())
                    {
                        _repositoryPhoneNumber.Delete(phoneNumber);
                    }
                }
                else
                {
                    // Remove phone numbers that are not in the new list
                    var phoneNumbersToRemove = user.OtherPhoneNumbers
                        .Where(p => !dto.OtherPhoneNumbers.Contains(p.PhoneNumber))
                        .ToList();

                    foreach (var phoneNumber in phoneNumbersToRemove)
                    {
                        _repositoryPhoneNumber.Delete(phoneNumber);
                    }

                    // Add new phone numbers
                    foreach (var phoneNumber in dto.OtherPhoneNumbers)
                    {
                        if (!user.OtherPhoneNumbers.Any(p => p.PhoneNumber == phoneNumber))
                        {
                            var newPhoneNumber = new UserPhoneNumber
                            {
                                PhoneNumber = phoneNumber,
                                IsActive = true,
                                UserId = user.Id,
                                User = user // Set the navigation property
                            };
                            user.OtherPhoneNumbers.Add(newPhoneNumber);
                        }
                    }
                }
            }

            // Handle roles
            if (dto.UserRoles != null)
            {
                // If the list is empty, remove all user roles
                if (dto.UserRoles.Count == 0)
                {
                    foreach (var userRole in user.UserRoles.ToList())
                    {
                        _repositoryUserRole.Delete(userRole);
                    }
                }
                else
                {
                    // Remove user roles that are not in the new list
                    var userRolesToRemove = user.UserRoles
                        .Where(r => !dto.UserRoles.Contains(r.RoleId.Value))
                        .ToList();

                    foreach (var userRole in userRolesToRemove)
                    {
                        _repositoryUserRole.Delete(userRole);
                    }

                    // Add new user roles
                    foreach (var roleId in dto.UserRoles)
                    {
                        if (!user.UserRoles.Any(r => r.RoleId == roleId))
                        {
                            var newUserRole = new UserRole
                            {
                                RoleId = roleId,
                                UserId = user.Id,
                                User = user // Set the navigation property
                            };
                            user.UserRoles.Add(newUserRole);
                        }
                    }
                }
            }

            // Update other user properties
            _mapper.Map(dto, user);
            _repository.Update(user);
            await _repository.SaveAsync();

            var result = _mapper.Map<UserDtoAdd>(user);
            return ApiResponse<UserDtoAdd>.Success(result, "ویرایش کاربر با موفقیت انجام شد.");
        }


        public async Task<ApiResponse<string>> DeleteAsync(int id)
        {
            var user = await _repository.GetByIdAsync(id, "OtherPhoneNumbers", "UserRoles", "UserRoles.Role");
            if (user == null) return ApiResponse<string>.Fail("کاربر یافت نشد.");

            //Delete all phone numbers of user
            if (user.OtherPhoneNumbers.Count > 0)
            {
                foreach (var phoneNumber in user.OtherPhoneNumbers.ToList())
                {
                    _repositoryPhoneNumber.Delete(phoneNumber);
                }
                await _repositoryPhoneNumber.SaveAsync();
            }

            //Delete all user roles
            if (user.UserRoles.Count > 0)
            {
                foreach (var userRole in user.UserRoles.ToList())
                {
                    _repositoryUserRole.Delete(userRole);
                }
                await _repositoryUserRole.SaveAsync();
            }

            _repository.Delete(user);
            await _repository.SaveAsync();

            return ApiResponse<string>.Success("کاربر با موفقیت حذف شد.");
        }
    }

}
