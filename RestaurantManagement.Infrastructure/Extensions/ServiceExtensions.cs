using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using RestaurantManagement.Domain.Interfaces.Repositories;
using RestaurantManagement.Domain.Interfaces.Security;
using RestaurantManagement.Infrastructure.Data;
using RestaurantManagement.Infrastructure.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Infrastructure.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped(typeof(RestaurantManagement.Domain.Interfaces.Repositories.IAsyncRepository<>), 
                typeof(EfRepository<>));

           // services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            // AutoMapper: scan assemblies for profiles
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<IRightResolver, RightResolver>();
            return services;
        }
    }   
}
