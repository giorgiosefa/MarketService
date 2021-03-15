using MarketService.Api.Infrastructure.Middleware;
using MarketService.Domain.Settings;
using MarketService.Repository.UnitOfWork;
using MarketService.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketService.Api.Infrastructure
{
    public static class ServiceExtensions
    {
        public static void ConfigureLocalServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMarketsService, MarketsService>();
            services.AddScoped<ICompanyService, CompanyService>();

            services.AddScoped<IUnitOfWorkManager, UnitOfWorkManager>();
        }

        public static void ConfigureCustomAuthorization(this IServiceCollection services, ApiSettings appSettings)
        {

            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }

        public static IApplicationBuilder UseCustomExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware(typeof(ExceptionMiddleware));
        }       
    }
}
