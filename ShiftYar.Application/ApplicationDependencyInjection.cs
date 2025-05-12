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
        }
    }
}