using ApplicationServices.Services;
using ApplicationServices.Utilities;
using DBService.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using MindUWebApi.Utilities;
using MindUWebApi.Filtros;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Serilog;
using Dtos.Interfaces;
using ApiService.Services;

namespace MindUWebApi
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
            services.AddDbContext<MindUContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Default")));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApiMindU", Version = "v1" });
                c.SwaggerDoc("v2", new OpenApiInfo { Title = "WebApiMindU", Version = "v2" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type= ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{ }
                    }
                });
            });

            services.AddAutoMapper(typeof(AutoMapperProfiles));
            services.AddHttpClient<IWeatherService, WeatherService>(client =>
            {
                client.BaseAddress = new Uri(Configuration["WeatherUrl"]);
            });
            services.AddHttpClient<IMapService, MapService>(client =>
            {
                client.BaseAddress = new Uri(Configuration["MapBoxURL"]);
            });

            services.AddTransient<AppServiceRoles>();
            services.AddTransient<AppServiceUsers>();
            services.AddTransient<AppServiceTechnologies>();
            services.AddTransient<AppServiceLevels>();
            services.AddTransient<AppServiceCollaborators>();
            services.AddTransient<AppServiceApis>();

            //services.AddResponseCaching();
            ConfigureAuth(services);
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(FiltroExcepcion));
                options.Conventions.Add(new SwaggerAgrupation());
            });

            services.AddCors(opciones =>
            {
                opciones.AddDefaultPolicy(builder =>
                {
                    //builder.WithOrigins("").AllowAnyMethod().AllowAnyHeader();    //Para especificar origen
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });

            services.AddApplicationInsightsTelemetry(Configuration["ApplicationInsights:ConnectionString"]);
            //IConfigurationRoot configuration = new ConfigurationBuilder()
            //    .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true).Build();
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(Configuration).CreateLogger();

        }

        protected virtual void ConfigureAuth(IServiceCollection services)
        {
            //Identificar a los usuarios
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["TokenKey"])),
                    ClockSkew = TimeSpan.Zero
                };
            });
            //Manejar los permisos por roles y token
            services.AddAuthorization(options =>
            {
                options.AddPolicy("SuperAdminPolicity", policy =>
                {
                    policy.RequireClaim("Role", new string[] { "SuperAdmin" });
                });
                options.AddPolicy("AdminPolicity", policy =>
                {
                    policy.RequireClaim("Role", new string[] { "Admin", "SuperAdmin" });
                });
                options.AddPolicy("UserPolicity", policy =>
                {
                    policy.RequireClaim("Role", new string[] { "User", "Admin", "SuperAdmin" });
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI( c=>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApiMindU V1");
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "WebApiMindU V2");
            });

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
