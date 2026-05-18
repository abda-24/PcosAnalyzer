using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Data.Context;
using Persistence.Data.ContextSeed;

namespace Persistence.Data.HallperClass
{
    public static class ApplicationRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            // 1. Database Configuration (SQL Server)
            services.AddDbContext<SmartPcosDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("BusinessConnection")));

            // 2. Identity Core Configuration
            services.AddIdentityCore<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<SmartPcosDbContext>();

            // 3. Register Data Seeding Service
            services.AddScoped<IDataSeeding, DataSeeding>();

            return services;
        }
    }
}