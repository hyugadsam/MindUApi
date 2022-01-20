using ApplicationServices.Services;
using ApplicationServices.Utilities;
using DBService.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using MindUWebApi.Loggers;

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
            services.AddDbContext<MindUContext>( options => options.UseSqlServer(Configuration.GetConnectionString("Default")) );
            //Logger por inyeccion de dependencias, pero no puede resolver la dependencia del dbContext al instanciar el logger provider
            //services.AddDbContextFactory<MindUContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Logs")) );
            //services.AddSingleton<ILoggerProvider, DbLoggerProvider>();

            services.AddSwaggerGen();
            services.AddAutoMapper(typeof(AutoMapperProfiles));
            services.AddTransient<AppServiceRoles>();
            services.AddTransient<AppServiceUsers>();
            services.AddResponseCaching();
            
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            var serviceProvider = app.ApplicationServices.CreateScope().ServiceProvider;
            var appDBContext = serviceProvider.GetRequiredService<MindUContext>();
            loggerFactory.AddProvider(new DbLoggerProvider(appDBContext));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
