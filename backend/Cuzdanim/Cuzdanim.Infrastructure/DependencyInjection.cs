using Cuzdanim.Application.Common.Interfaces;
using Cuzdanim.Application.Common.Models;
using Cuzdanim.Infrastructure.Data.Context;
using Cuzdanim.Infrastructure.Data.Interceptors;
using Cuzdanim.Infrastructure.Repositories;
using Cuzdanim.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cuzdanim.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // JWT Settings
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        // Interceptors
        services.AddScoped<DateTimeInterceptor>();

        // PostgreSQL bağlantısı
        services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
        {
            var dateTimeInterceptor = serviceProvider.GetRequiredService<DateTimeInterceptor>();

            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
            .AddInterceptors(dateTimeInterceptor);
        });

        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IBudgetRepository, BudgetRepository>();
        services.AddScoped<IGoalRepository, GoalRepository>();
        services.AddScoped<IRecurringTransactionRepository, RecurringTransactionRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>(); 

        // Services
        services.AddScoped<ICategorySeederService, CategorySeederService>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        return services;
    }
}