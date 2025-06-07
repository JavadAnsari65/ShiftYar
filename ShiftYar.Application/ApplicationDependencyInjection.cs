using Microsoft.Extensions.DependencyInjection;
using ShiftYar.Application.Common.Mappings;
using ShiftYar.Application.Features.DepartmentModel.Services;
using ShiftYar.Application.Features.PermissionModel.Services;
using ShiftYar.Application.Features.RoleModel.Services;
using ShiftYar.Application.Features.UserModel.Services;
using ShiftYar.Application.Interfaces.DepartmentModel;
using ShiftYar.Application.Interfaces.RoleModel;
using ShiftYar.Application.Interfaces.Security;
using ShiftYar.Application.Interfaces.UserModel;
using ShiftYar.Application.Interfaces.PermissionModel;
using System.Security;
using ShiftYar.Application.Interfaces.RolePermissionModel;
using ShiftYar.Application.Features.RolePermissionModel.Services;
using ShiftYar.Application.Interfaces.SpecialtyModel;
using ShiftYar.Application.Features.SpecialtyModel.Services;
using ShiftYar.Application.Interfaces.ShiftRequiredSpecialtyModel;
using ShiftYar.Application.Features.ShiftRequiredSpecialtyModel.Services;
using ShiftYar.Application.Interfaces.ShiftModel;
using ShiftYar.Application.Features.ShiftModel.Services;
using ShiftYar.Application.Interfaces.FileUploaderInterface;
using ShiftYar.Application.Features.FileUploader.Services;
using ShiftYar.Application.Features.CalendarSeeder.Services;
using ShiftYar.Application.Interfaces.CalendarSeeder;
using ShiftYar.Application.Interfaces.AddressModel;
using ShiftYar.Application.Features.AddressModel.Services;

namespace ShiftYar.Application
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // AutoMapper
            services.AddAutoMapper(typeof(UserProfile).Assembly);

            // Services
            services.AddApplicationServices();

            return services;
        }

        private static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IRolePermissionService, RolePermissionService>();
            services.AddScoped<ISpecialtyService, SpecialtyService>();
            services.AddScoped<IShiftRequiredSpecialtyService, ShiftRequiredSpecialtyService>();
            services.AddScoped<IShiftService, ShiftService>();
            services.AddScoped<IFileUploader, FileUploaderService>();
            services.AddScoped<ICalendarSeederService, CalendarSeederService>();
            services.AddScoped<IProvinceService, ProvinceService>();
            services.AddScoped<ICityService, CityService>();
        }
    }
}