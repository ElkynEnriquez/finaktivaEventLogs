using FinaktivaEventLogs.Domain.Interfaces;
using FinaktivaEventLogs.Infrastructure;
using FinaktivaEventLogs.Infrastructure.Data;
using FinaktivaEventLogs.Services;
using FinaktivaEventLogs.Services.Common;
using FinaktivaEventLogs.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinaktivaEventLogs.API.Extensions
{
    public static class ServicesCollectionExtensions
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IEventLogService, EventLogService>();
        }
        public static void AddLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }
        public static void AddDbConfiguration(this IServiceCollection services, string connectionString)
        {
            // For DataBase MS SqlServer
            services.AddDbContext<ApplicationDbContext>(
                option => option.UseSqlServer(connectionString)
                                .EnableSensitiveDataLogging()
                );
            // For Database PostgreSQL
            //services.AddDbContext<ApplicationDbContext>(
            //    option => option.UseNpgsql(connectionString)
            //                    .EnableSensitiveDataLogging()
            //    );
        }
        public static void AddCorsDomainConfiguration(this IServiceCollection services, string MyAllowSpecificOrigins)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4200");
                        builder.AllowAnyMethod();
                        builder.AllowAnyHeader();
                    });
            });
        }
    }
}
