using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShiftYar.Application.Interfaces.Persistence;
using ShiftYar.Infrastructure.Persistence.AppDbContext;
using ShiftYar.Infrastructure.Persistence.Repositories;
using ShiftYar.Infrastructure.Persistence.Repositories.HospitalModel;
using ShiftYar.Infrastructure.Services.Security;
using ShiftYar.Application.Interfaces.HospitalModel;
using ShiftYar.Application.Interfaces.Security;
using ShiftYar.Application.Interfaces.CalendarSeeder;
using ShiftYar.Infrastructure.Services.CalendarSeeder;

namespace ShiftYar.Infrastructure
{
    public static class InfrastructureDependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Database Context
            services.AddDbContext(configuration);

            // Repositories
            services.AddRepositories();

            // Services
            services.AddInfrastructureServices();

            return services;
        }

        private static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ShiftYarDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("ShiftYarDbConnection")));
        }

        private static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IEfRepository<>), typeof(EfRepository<>));
        }

        private static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IHospitalService, HospitalService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<ICalendarSeederService, CalendarSeederService>();

        }
    }
}