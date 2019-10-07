using App.Api.Auth;
using App.Api.Middleware;
using App.Biz.Auth;
using App.Biz.Services;
using App.Biz.Settings;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Entities;
using System.IO;

namespace App.Api
{
    public class Startup
    {
        private readonly AppSettings settings = new AppSettings();
        private readonly IWebHostEnvironment environment;

        public Startup(IConfiguration appsettings, IWebHostEnvironment environment)
        {
            appsettings.Bind("Settings", settings);
            this.environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(settings);
            services.AddHostedService<EmailService>();
            services.AddMongoDBEntities(settings.Database.Name);
            services.AddCors();
            services.AddRouting();
            services.AddControllers()
                    .AddNewtonsoftJson(o => o.UseMemberCasing())
                    .AddFluentValidation(o => o.RegisterValidatorsFromAssemblyContaining<AppSettings>())
                    .ConfigureApiBehaviorOptions(o => o.SuppressModelStateInvalidFilter = false); //set to true to disable automatic model error response 400
            services.AddDataProtection()
                    .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(environment.ContentRootPath, "dp_keys")));
            services.AddJWTAuthentication(settings);
            services.AddAuthorization(o => o.EnablePolicies());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions //needed for nginx reverse proxy
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            if (env.IsDevelopment())
            {
                app.UseCors(b => b.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
                app.UseDeveloperExceptionPage();
            }
            app.UseMaintenanceModeMiddleware();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseJWTAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(b => b.MapControllers());

            //note: the order of the above is important
        }
    }
}