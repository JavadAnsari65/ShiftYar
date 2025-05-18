﻿using AutoMapper;
using Microsoft.Extensions.Logging;
using ShiftYar.Application.Common.Models.ResponseModel;
using ShiftYar.Application.DTOs.UserModel;
using ShiftYar.Application.Features.UserModel.Filters;
using ShiftYar.Application.Interfaces.Persistence;
using ShiftYar.Application.Interfaces.UserModel;
using ShiftYar.Domain.Entities.UserModel;

namespace ShiftYar.Application.Features.UserModel.Services
{
    public class UserService : IUserService
    {
        private readonly IEfRepository<User> _repository;
        private readonly IEfRepository<UserPhoneNumber> _repositoryPhoneNumber;
        private readonly IEfRepository<UserRole> _repositoryUserRole;
        private readonly IMapper _mapper; // AutoMapper
        private readonly ILogger<UserService> _logger;

        public UserService(IEfRepository<User> repository, IEfRepository<UserPhoneNumber> repositoryPhoneNumber,
                                IEfRepository<UserRole> repositoryUserRole, IMapper mapper, ILogger<UserService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _repositoryPhoneNumber = repositoryPhoneNumber;
            _repositoryUserRole = repositoryUserRole;
            _logger = logger;
        }

        /// Get All User
        public async Task<ApiResponse<PagedResponse<UserDtoGet>>> GetFilteredUsersAsync(UserFilter filter)
        {
            try
            {
                _logger.LogInformation("Fetching users with filter: {@Filter}", filter);
                var result = await _repository.GetByFilterAsync(
                    filter,
                    "OtherPhoneNumbers",
                    "UserRoles",
                    "UserRoles.Role",
                    "Department",
                    "Specialty"
                );
                var data = _mapper.Map<List<UserDtoGet>>(result.Items);

                var pagedResponse = new PagedResponse<UserDtoGet>
                {
                    Items = data,
                    TotalCount = result.TotalCount,
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    TotalPages = (int)Math.Ceiling(result.TotalCount / (double)filter.PageSize)
                };

                _logger.LogInformation("Successfully fetched {Count} users out of {TotalCount}", data.Count, result.TotalCount);
                return ApiResponse<PagedResponse<UserDtoGet>>.Success(pagedResponse);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Failed Fetching users with filter: {@Filter}", filter);
                throw new Exception("سرویس دریافت لیست کاربران با خطا مواجه شد : " + ex.Message);
            }
        }


        ///Get User
        public async Task<ApiResponse<UserDtoAdd>> GetByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Fetching user with ID: {Id}", id);
                var user = await _repository.GetByIdAsync(id, "OtherPhoneNumbers", "UserRoles", "UserRoles.Role", "Department", "Specialty");
                if (user == null)
                {
                    _logger.LogWarning("User with ID {Id} not found", id);
                    return ApiResponse<UserDtoAdd>.Fail("کاربر یافت نشد.");
                }

                var dto = _mapper.Map<UserDtoAdd>(user);
                _logger.LogInformation("Successfully fetched user with ID: {Id}", id);
                return ApiResponse<UserDtoAdd>.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Failed Fetching The user by ID: {Id}", id);
                throw new Exception("سرویس دریافت کاربر موردنظر با خطا مواجه شد : " + ex.Message);
            }
        }


        ///Create User
        public async Task<ApiResponse<UserDtoAdd>> CreateAsync(UserDtoAdd dto)
        {
            try
            {
                _logger.LogInformation("Creating new user with NationalCode: {NationalCode}", dto.NationalCode);

                if (!string.IsNullOrEmpty(dto.NationalCode))
                {
                    var exists = await _repository.ExistsAsync(u => u.NationalCode == dto.NationalCode);
                    if (exists)
                    {
                        _logger.LogWarning("User creation failed - NationalCode {NationalCode} already exists", dto.NationalCode);
                        return ApiResponse<UserDtoAdd>.Fail("کاربری با این کد ملی وجود دارد.");
                    }
                }

                var user = _mapper.Map<User>(dto);
                await _repository.AddAsync(user);
                await _repository.SaveAsync();

                var result = _mapper.Map<UserDtoAdd>(user);
                _logger.LogInformation("Successfully created user with ID: {Id}", user.Id);
                return ApiResponse<UserDtoAdd>.Success(result, "کاربر با موفقیت ایجاد شد.");
            }
            catch (Exception ex)
            {
                _logger.LogWarning("User creation failed - NationalCode {NationalCode}", dto.NationalCode);
                throw new Exception("سرویس ایجاد کاربر با خطا مواجه شد : " + ex.Message);
            }
        }


        ///Update User
        public async Task<ApiResponse<UserDtoAdd>> UpdateAsync(int id, UserDtoAdd dto)
        {
            try
            {
                _logger.LogInformation("Updating user with ID: {Id}", id);

                var user = await _repository.GetByIdAsync(id, "OtherPhoneNumbers", "UserRoles", "UserRoles.Role", "Department", "Specialty");
                if (user == null)
                {
                    _logger.LogWarning("User update failed - User with ID {Id} not found", id);
                    return ApiResponse<UserDtoAdd>.Fail("کاربر یافت نشد.");
                }

                // Handle phone numbers
                if (dto.OtherPhoneNumbers != null)
                {
                    _logger.LogInformation("Updating phone numbers for user {Id}", id);
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
                    _logger.LogInformation("Updating roles for user {Id}", id);
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
                _logger.LogInformation("Successfully updated user with ID: {Id}", id);
                return ApiResponse<UserDtoAdd>.Success(result, "ویرایش کاربر با موفقیت انجام شد.");
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Failed updated user with ID: {Id}", id);
                throw new Exception("سرویس ویرایش کاربر با خطا مواجه شد : " + ex.Message);
            }
        }


        public async Task<ApiResponse<string>> DeleteAsync(int id)
        {
            try
            {
                _logger.LogInformation("Deleting user with ID: {Id}", id);

                var user = await _repository.GetByIdAsync(id, "OtherPhoneNumbers", "UserRoles", "UserRoles.Role");
                if (user == null)
                {
                    _logger.LogWarning("User deletion failed - User with ID {Id} not found", id);
                    return ApiResponse<string>.Fail("کاربر یافت نشد.");
                }

                //Delete all phone numbers of user
                if (user.OtherPhoneNumbers.Count > 0)
                {
                    _logger.LogInformation("Deleting {Count} phone numbers for user {Id}", user.OtherPhoneNumbers.Count, id);
                    foreach (var phoneNumber in user.OtherPhoneNumbers.ToList())
                    {
                        _repositoryPhoneNumber.Delete(phoneNumber);
                    }
                    await _repositoryPhoneNumber.SaveAsync();
                }

                //Delete all user roles
                if (user.UserRoles.Count > 0)
                {
                    _logger.LogInformation("Deleting {Count} roles for user {Id}", user.UserRoles.Count, id);
                    foreach (var userRole in user.UserRoles.ToList())
                    {
                        _repositoryUserRole.Delete(userRole);
                    }
                    await _repositoryUserRole.SaveAsync();
                }

                _repository.Delete(user);
                await _repository.SaveAsync();

                _logger.LogInformation("Successfully deleted user with ID: {Id}", id);
                return ApiResponse<string>.Success("کاربر با موفقیت حذف شد.");
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Failed Delete user with ID: {Id}", id);
                throw new Exception("سرویس حذف کاربر با خطا مواجه شد : " + ex.Message);
            }
        }
    }
}
