﻿using AccessControlSystem.Application.AutoMapper.Abstraction;
using AccessControlSystem.Application.AutoMapper.AccessGroupDevices;
using AccessControlSystem.Application.AutoMapper.AccessGroups;
using AccessControlSystem.Application.AutoMapper.Cards;
using AccessControlSystem.Application.AutoMapper.Devices;
using AccessControlSystem.Application.AutoMapper.Roles;
using AccessControlSystem.Application.AutoMapper.Shared;
using AccessControlSystem.Application.AutoMapper.Subscriptions;
using AccessControlSystem.Application.AutoMapper.Units;
using AccessControlSystem.Application.AutoMapper.Users;
using AccessControlSystem.Application.Configurations;
using AccessControlSystem.Application.Interfaces.Abstraction;
using AccessControlSystem.Application.Interfaces.AccessGroupDevices;
using AccessControlSystem.Application.Interfaces.AccessGroups;
using AccessControlSystem.Application.Interfaces.Cards;
using AccessControlSystem.Application.Interfaces.Devices;
using AccessControlSystem.Application.Interfaces.Roles;
using AccessControlSystem.Application.Interfaces.Shared;
using AccessControlSystem.Application.Interfaces.Subscriptions;
using AccessControlSystem.Application.Interfaces.Units;
using AccessControlSystem.Application.Interfaces.Users;
using AccessControlSystem.Application.Services.Abstraction;
using AccessControlSystem.Application.Services.AccessGroupDevices;
using AccessControlSystem.Application.Services.AccessGroups;
using AccessControlSystem.Application.Services.Cards;
using AccessControlSystem.Application.Services.Devices;
using AccessControlSystem.Application.Services.Roles;
using AccessControlSystem.Application.Services.Subscriptions;
using AccessControlSystem.Application.Services.Units;
using AccessControlSystem.Application.Services.Users;
using AccessControlSystem.Application.Validators.AccessGroupDevices;
using AccessControlSystem.Application.Validators.AccessGroups;
using AccessControlSystem.Application.Validators.Cards;
using AccessControlSystem.Application.Validators.Devices;
using AccessControlSystem.Application.Validators.Roles;
using AccessControlSystem.Application.Validators.Subscriptions;
using AccessControlSystem.Application.Validators.Units;
using AccessControlSystem.Application.Validators.Users;
using AccessControlSystem.Common.Tokens.Configurations;
using AccessControlSystem.Common.Tokens.Interfaces;
using AccessControlSystem.Common.Tokens.Services;
using AccessControlSystem.Domain.Constants;
using AccessControlSystem.Domain.Interfaces.Repositories.Abstraction;
using AccessControlSystem.Domain.Interfaces.Repositories.AccessGroupDevices;
using AccessControlSystem.Domain.Interfaces.Repositories.AccessGroups;
using AccessControlSystem.Domain.Interfaces.Repositories.Cards;
using AccessControlSystem.Domain.Interfaces.Repositories.Devices;
using AccessControlSystem.Domain.Interfaces.Repositories.Roles;
using AccessControlSystem.Domain.Interfaces.Repositories.Subscriptions;
using AccessControlSystem.Domain.Interfaces.Repositories.Units;
using AccessControlSystem.Domain.Interfaces.Repositories.Users;
using AccessControlSystem.Domain.Interfaces.Specifications.Absraction;
using AccessControlSystem.Domain.Interfaces.UnitOfWork;
using AccessControlSystem.Domain.Models.Roles;
using AccessControlSystem.Domain.Models.Users;
using AccessControlSystem.Domain.Services.Subscriptions;
using AccessControlSystem.Domain.Specifications.Absraction;
using AccessControlSystem.Infrastructure.Data.Context;
using AccessControlSystem.Infrastructure.Data.Repositories.Abstraction;
using AccessControlSystem.Infrastructure.Data.Repositories.AccessGroupDevices;
using AccessControlSystem.Infrastructure.Data.Repositories.AccessGroups;
using AccessControlSystem.Infrastructure.Data.Repositories.Cards;
using AccessControlSystem.Infrastructure.Data.Repositories.Devices;
using AccessControlSystem.Infrastructure.Data.Repositories.Roles;
using AccessControlSystem.Infrastructure.Data.Repositories.Subscriptions;
using AccessControlSystem.Infrastructure.Data.Repositories.Units;
using AccessControlSystem.Infrastructure.Data.Repositories.Users;
using AccessControlSystem.Infrastructure.Data.UnitOfWork;
using AccessControlSystem.Infrastructure.Http.Clients.Airfob;
using AccessControlSystem.Infrastructure.Http.Configurations;
using AccessControlSystem.Infrastructure.Http.Handlers.Airfob;
using AccessControlSystem.Infrastructure.Http.Interfaces.Airfob;
using AccessControlSystem.Infrastructure.Http.Services.Airfob;
using AccessControlSystem.WebApi.Middlewares.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http.Headers;
using System.Text;

namespace AccessControlSystem.Infrastructure.IoC.DependencyContainer;

public static class DependencyContainer
{
    public static void RegisterConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtTokenSettings>(configuration.GetSection(JwtTokenSettings.SectionName));

        services.Configure<ImageSettings>(configuration.GetSection(ImageSettings.SectionName));

        services.Configure<AirfobSettings>(configuration.GetSection("AirfobSettings"));
    }

    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IBaseService<,,>), typeof(BaseService<,,>))
            .AddScoped<ITokensService, TokensService>()
            .AddScoped<IUserService, UserService>()
            .AddScoped<IRoleService, RoleService>()
            .AddScoped<ISubscriptionService, SubscriptionService>()
            .AddScoped<IDeviceService, DeviceService>()
            .AddScoped<IUnitService, UnitService>()
            .AddScoped<IAccessGroupService, AccessGroupService>()
            .AddScoped<IAccessGroupDeviceService, AccessGroupDeviceService>()
            .AddScoped<IImageService, ImageService>()
            .AddScoped<ICardService, CardService>();
    }

    public static void RegisterDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AccessControlDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });
    }

    public static void RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IBaseRepository<,>), typeof(BaseRepository<,>))
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IRoleRepository, RoleRepository>()
            .AddScoped<ISubscriptionRepository, SubscriptionRepository>()
            .AddScoped<IDeviceRepository, DeviceRepository>()
            .AddScoped<IUnitRepository, UnitRepository>()
            .AddScoped<IAccessGroupRepository, AccessGroupRepository>()
            .AddScoped<IAccessGroupDeviceRepository, AccessGroupDeviceRepository>()
            .AddScoped<ICardRepository, CardRepository>();
    }

    public static void RegisterSpecifications(this IServiceCollection services)
    {
        services.AddScoped(typeof(IBaseSpecification<>), typeof(BaseSpecification<>))
            .AddScoped(typeof(ISpecificationCombiner<>), typeof(SpecificationCombiner<>));
    }

    public static void RegisterStrategies(this IServiceCollection services)
    {
        services.AddSingleton<SubscriptionValidationStrategyFactory>();
    }

    public static void RegisterUnitOfWork(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    public static void RegisterAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(BaseModelProfile).Assembly);
        services.AddAutoMapper(typeof(PaginatedModelProfile).Assembly);
        services.AddAutoMapper(typeof(UserProfile).Assembly);
        services.AddAutoMapper(typeof(RoleProfile).Assembly);
        services.AddAutoMapper(typeof(SubscriptionProfile).Assembly);
        services.AddAutoMapper(typeof(DeviceProfile).Assembly);
        services.AddAutoMapper(typeof(UnitProfile).Assembly);
        services.AddAutoMapper(typeof(AccessGroupProfile).Assembly);
        services.AddAutoMapper(typeof(AccessGroupDeviceProfile).Assembly);
        services.AddAutoMapper(typeof(CardProfile).Assembly);
    }

    public static void RegisterValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<UserDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<LoginDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<ResetPasswordDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<ForgotPasswordDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<RoleDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<SubscriptionDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<DeviceDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<UnitDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<AccessGroupDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<AccessGroupDeviceDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<CardDtoValidator>();
    }

    public static void RegisterIdentity(this IServiceCollection services)
    {
        services.AddIdentity<User, Role>()
            .AddEntityFrameworkStores<AccessControlDbContext>()
            .AddDefaultTokenProviders();
    }

    public static void RegisterJwtSettings(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtTokens");
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["ValidIssuer"],
                ValidAudience = jwtSettings["ValidAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!)),
                ClockSkew = new TimeSpan(0, 2, 0)
            };
        });
    }

    public static void RegisterCORS(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()!;

            options.AddPolicy(AppSettings.AllowedOrigins,
                policy => policy.WithOrigins(allowedOrigins)
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                .AllowCredentials());
        });
    }

    public static void ApplyMigrations(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<AccessControlDbContext>();

        dbContext.Database.Migrate();
    }

    public static void RegisterMiddlewares(this IServiceCollection services)
    {
        services.AddTransient<ExceptionHandlingMiddleware>();
    }

    public static IServiceCollection AddHttpServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAirfobAuthService, AirfobAuthService>();
        services.AddScoped<IAirfobService, AirfobService>();

        services.AddTransient<AirfobAuthHandler>();

        services.AddHttpClient<AirfobClient>(client =>
        {
            client.BaseAddress = new Uri(configuration["AirfobSettings:BaseUrl"]!);
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        })
        .AddHttpMessageHandler<AirfobAuthHandler>();

        return services;
    }
}
