﻿using AutoMapper;
using ShiftYar.Application.DTOs.UserModel;
using ShiftYar.Domain.Entities.RoleModel;
using ShiftYar.Domain.Entities.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftYar.Application.Common.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDtoAdd>()
                .ForMember(dest => dest.OtherPhoneNumbers, opt => opt.MapFrom(src =>
                    src.OtherPhoneNumbers != null ? src.OtherPhoneNumbers.Select(p => p.PhoneNumber).ToList() : null))
                .ForMember(dest => dest.UserRoles, opt => opt.MapFrom(src =>
                    src.UserRoles != null ? src.UserRoles.Select(r => r.RoleId).ToList() : null));

            CreateMap<UserDtoAdd, User>()
                .ForMember(dest => dest.OtherPhoneNumbers, opt => opt.MapFrom(src =>
                    src.OtherPhoneNumbers != null ? src.OtherPhoneNumbers.Select(p => new UserPhoneNumber { PhoneNumber = p }).ToList() : null))
                .ForMember(dest => dest.UserRoles, opt => opt.MapFrom(src =>
                    src.UserRoles != null ? src.UserRoles.Select(r => new UserRole { RoleId = r }).ToList() : null));


            //CreateMap<User, UserDtoGet>()
            //    .ForMember(dest => dest.OtherPhoneNumbers, opt => opt.MapFrom(src => src.OtherPhoneNumbers))
            //    .ForMember(dest => dest.UserRoles, opt => opt.MapFrom(src => src.UserRoles));

            //CreateMap<UserDtoGet, User>()
            //    .ForMember(dest => dest.OtherPhoneNumbers, opt => opt.MapFrom(src => src.OtherPhoneNumbers))
            //    .ForMember(dest => dest.UserRoles, opt => opt.MapFrom(src => src.UserRoles));

            CreateMap<UserPhoneNumber, UserPhoneNumber>()
                .ForMember(dest => dest.User, opt => opt.Ignore());

            CreateMap<UserRole, UserRole>()
                .ForMember(dest => dest.User, opt => opt.Ignore());

            //CreateMap<User, UserDtoGet>()
            //    .ForMember(dest => dest.OtherPhoneNumbers, opt => opt.MapFrom(src =>
            //        src.OtherPhoneNumbers.Select(p => new UserPhoneNumber
            //        {
            //            Id = p.Id,
            //            PhoneNumber = p.PhoneNumber,
            //            UserId = p.UserId
            //        }).ToList()))
            //    .ForMember(dest => dest.UserRoles, opt => opt.MapFrom(src =>
            //        src.UserRoles.Select(r => new UserRole
            //        {
            //            Id = r.Id,
            //            UserId = r.UserId,
            //            RoleId = r.RoleId,
            //            Role = r.Role
            //        }).ToList()));

            //CreateMap<UserDtoGet, User>()
            //    .ForMember(dest => dest.OtherPhoneNumbers, opt => opt.MapFrom(src =>
            //        src.OtherPhoneNumbers.Select(p => new UserPhoneNumber
            //        {
            //            Id = p.Id,
            //            PhoneNumber = p.PhoneNumber,
            //            UserId = p.UserId
            //        }).ToList()))
            //    .ForMember(dest => dest.UserRoles, opt => opt.MapFrom(src =>
            //        src.UserRoles.Select(r => new UserRole
            //        {
            //            Id = r.Id,
            //            UserId = r.UserId,
            //            RoleId = r.RoleId,
            //            Role = r.Role
            //        }).ToList()));

            CreateMap<User, UserDtoGet>()
                .ForMember(dest => dest.OtherPhoneNumbers, opt => opt.MapFrom(src =>
                    src.OtherPhoneNumbers.Select(p => new UserPhoneNumber
                    {
                        Id = p.Id,
                        PhoneNumber = p.PhoneNumber,
                        UserId = p.UserId,
                        IsActive = p.IsActive
                    }).ToList()))
                .ForMember(dest => dest.UserRoles, opt => opt.MapFrom(src =>
                    src.UserRoles.Select(r => new UserRole
                    {
                        Id = r.Id,
                        UserId = r.UserId,
                        RoleId = r.RoleId,
                        Role = r.Role != null ? new Role
                        {
                            Id = r.Role.Id,
                            Name = r.Role.Name,
                            IsActive = r.Role.IsActive
                        } : null
                    }).ToList()));

            CreateMap<UserDtoGet, User>()
                .ForMember(dest => dest.OtherPhoneNumbers, opt => opt.MapFrom(src =>
                    src.OtherPhoneNumbers.Select(p => new UserPhoneNumber
                    {
                        Id = p.Id,
                        PhoneNumber = p.PhoneNumber,
                        UserId = p.UserId,
                        IsActive = p.IsActive
                    }).ToList()))
                .ForMember(dest => dest.UserRoles, opt => opt.MapFrom(src =>
                    src.UserRoles.Select(r => new UserRole
                    {
                        Id = r.Id,
                        UserId = r.UserId,
                        RoleId = r.RoleId,
                        Role = r.Role != null ? new Role
                        {
                            Id = r.Role.Id,
                            Name = r.Role.Name,
                            IsActive = r.Role.IsActive
                        } : null
                    }).ToList()));
        }
    }
}
