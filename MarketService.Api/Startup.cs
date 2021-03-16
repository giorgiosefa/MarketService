using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketService.Api.Infrastructure;
using MarketService.Domain.Settings;
using MarketService.Persistence.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MarketService.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //CORS
            var corsBuilder = new CorsPolicyBuilder();
            corsBuilder.AllowAnyHeader();
            corsBuilder.AllowAnyMethod();
            corsBuilder.AllowAnyOrigin(); // For everyone
           
            //corsBuilder.WithOrigins("http://localhost:8080"); 

            corsBuilder.AllowCredentials();

            services.AddCors(options =>
            {
                options.AddPolicy("MyCorsPolicy", corsBuilder.Build());
            });

            services.ConfigureLocalServices();

            string dbConnectionString = GetConnectionString();
            services.AddDbContext<MarketDbContext>(options => options.UseSqlServer(dbConnectionString));

            IConfigurationSection appSettingsSection = Configuration.GetSection("ApiSettings");
            services.Configure<ApiSettings>(appSettingsSection);
            ApiSettings appSettings = appSettingsSection.Get<ApiSettings>();

            services.ConfigureCustomAuthorization(appSettings);

            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCustomExceptionMiddleware();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();

            app.UseMvc();

            
            using (IServiceScope serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                MarketDbContext context = serviceScope.ServiceProvider.GetService<MarketDbContext>();
                context.Database.Migrate();
                DbInitializer.Initialize(context);
            }
            
            app.UseSwagger();            
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Market API V1");
            });
        }

        private string GetConnectionString()
        {
            string conStr = Configuration.GetConnectionString("MarketDbContext");
            return conStr;
        }
    }
}
