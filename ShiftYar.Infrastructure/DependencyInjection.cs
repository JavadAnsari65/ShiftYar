using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShiftYar.Application.Common.Mappings;
using ShiftYar.Application.Features.UserModel.Services;
using ShiftYar.Application.Interfaces.HospitalModel;
using ShiftYar.Application.Interfaces.Persistence;
using ShiftYar.Application.Interfaces.Security;
using ShiftYar.Application.Interfaces.UserModel;
using ShiftYar.Infrastructure.Persistence.AppDbContext;
using ShiftYar.Infrastructure.Persistence.Repositories;
using ShiftYar.Infrastructure.Persistence.Repositories.HospitalModel;
using ShiftYar.Infrastructure.Services.Security;
using System;


namespace ShiftYar.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // تنظیم DbContext
            services.AddDbContext<ShiftYarDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("ShiftYarDbConnection")));

            // ریپازیتوری عمومی
            services.AddScoped(typeof(IEfRepository<>), typeof(EfRepository<>));

            return services;
        }
    }
}
