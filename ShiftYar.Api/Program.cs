using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.File;
using ShiftYar.Api.Filters;
using ShiftYar.Application.Features.UserModel.Services;
using ShiftYar.Application.Interfaces.HospitalModel;
using ShiftYar.Application.Interfaces.Security;
using ShiftYar.Application.Interfaces.UserModel;
using ShiftYar.Infrastructure.Persistence.AppDbContext;
using ShiftYar.Infrastructure.Persistence.Repositories.HospitalModel;
using ShiftYar.Infrastructure.Services.Security;
using ShiftYar.Infrastructure;
using System;
using System.Text;
using ShiftYar.Application.Common.Mappings;
using ShiftYar.Application.Interfaces.Persistence;
using ShiftYar.Infrastructure.Persistence.Repositories;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .Enrich.WithThreadId()
    .Enrich.WithEnvironmentName()
);

try
{
    Log.Information("Starting web application");

    // Add services
    builder.Services.AddControllers(options =>
    {
        ///این دوتا رو به صورت گلوبال در Program.cs رجیستر کردیم 
        ///یعنی روی همه‌ی کنترلرها و اکشن‌ها به طور پیش‌فرض اعمال می‌شن. نیازی نیست جایی جداگانه بنویسی.
        options.Filters.Add<GlobalExceptionFilter>();
        options.Filters.Add<ValidateModelFilter>();
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

    // ثبت DIها
    //builder.Services.AddApplication();
    //builder.Services.AddInfrastructure(builder.Configuration);

    //Add Cors
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAllOrigins",
            builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
    });

    // Add Swagger
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "ShiftYar API",
            Version = "v1",
            Description = "API for ShiftYar application"
        });

        // Add JWT Authentication
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    });

    builder.Services.AddScoped<IHospitalService, HospitalService>();
    builder.Services.AddScoped<IJwtService, JwtService>();

    //DB Context
    builder.Services.AddDbContext<ShiftYarDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("ShiftYarDbConnection")));
    builder.Services.AddAutoMapper(typeof(UserProfile).Assembly);
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped(typeof(IEfRepository<>), typeof(EfRepository<>));

    builder.Services.AddScoped<RequestLoggingFilter>();
    builder.Services.AddScoped<GlobalExceptionFilter>();
    builder.Services.AddScoped<ValidateModelFilter>();
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            };
        });
    builder.Services.AddAuthorization();

    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    //builder.Services.AddOpenApi();

    var app = builder.Build();

    // Middleware
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "ShiftYar API V1");
            c.RoutePrefix = string.Empty; // swagger در root باز شود
        });
    }

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

