using Microsoft.Extensions.DependencyInjection;
using ShiftYar.Application.Common.Mappings;
using ShiftYar.Application.Features.UserModel.Services;
using ShiftYar.Application.Interfaces.UserModel;

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
        }
    }
}