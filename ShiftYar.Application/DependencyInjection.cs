using Microsoft.Extensions.DependencyInjection;
using ShiftYar.Application.Common.Mappings;
using ShiftYar.Application.Features.UserModel.Services;
using ShiftYar.Application.Interfaces.HospitalModel;
using ShiftYar.Application.Interfaces.UserModel;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // سرویس‌های Application
            //services.AddScoped<IHospitalService, HospitalService>();
            services.AddScoped<IUserService, UserService>();
            services.AddAutoMapper(typeof(DependencyInjection).Assembly);

            // اضافه کردن AutoMapper
            services.AddAutoMapper(typeof(DependencyInjection).Assembly);

            return services;
        }
    }
}
