using ApplicationServices.Utilities;
using AutoMapper;
using DBService.Entities;
using Dtos.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MindUWebApi.Test.Utilidades;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MindUWebApi.Test
{
    public class BasePruebas<T>
    {
        private static readonly string urlLogin = "api/v1/Users/Login";

        protected MindUContext ConstruirContext(string name)
        {
            var opciones = new DbContextOptionsBuilder<MindUContext>()
                .UseInMemoryDatabase(name).Options;

            var dbContext = new MindUContext(opciones);
            return dbContext;

        }

        protected IMapper ConfigureMapper()
        {
            var config = new MapperConfiguration(options =>
            {
                options.AddProfile(new AutoMapperProfiles());
            });
            return config.CreateMapper();

        }

        protected ILogger<T> ConfigureLogger()
        {
            var mok = Mock.Of<ILogger<T>>();

            return mok;
        }

        protected IConfigurationRoot GetConfiguration()
        {
            return new ConfigurationBuilder().AddJsonFile("appSettings.json", optional: false, reloadOnChange: true).Build();
        }

        public async Task<string> GetLoginToken(string nombre)
        {
            var context = ConstruirContext(nombre);

            context.Users.Add(new DBService.Entities.Users
            {
                Email = "admin@example.com",
                IsActive = true,
                Name = "admin",
                Password = "9OfdEQe9YyV9KmWuHI65quf11Ck=",
                RoleId = 1,
                Salt = "OhMYYGdTCcATbL+/nlxMynyB4s0="
            });

            await context.SaveChangesAsync();

            var factory = GetWebAppFactory(nombre);

            var client = factory.CreateClient();
            var request = new Dtos.Request.LoginRequest
            {
                Email = "admin@example.com",
                Password = "aaaaa",
            };
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var respuest = await client.PostAsync(urlLogin, httpContent);

            ////Prueba
            respuest.EnsureSuccessStatusCode();
            var response = JsonConvert.DeserializeObject<LoginResponse>(await respuest.Content.ReadAsStringAsync());
            return response.Token;

        }

        protected WebApplicationFactory<Startup> GetWebAppFactory(string DBName, bool ignorarSeguridad = true)
        {
            var factory = new WebApplicationFactory<Startup>();
            
            factory = factory.WithWebHostBuilder(webHostBuilder =>
            {
               // webHostBuilder.ConfigureAppConfiguration((webApplication, config) =>
               //{
               //    webApplication.Configuration = GetConfiguration();
               //});

                webHostBuilder.ConfigureTestServices(services =>
                {
                    var descriptorDBContext = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<MindUContext>));
                    if (descriptorDBContext != null)
                    {
                        services.Remove(descriptorDBContext);
                    }

                    services.AddDbContext<MindUContext>(options => options.UseInMemoryDatabase(DBName));

                    if (ignorarSeguridad)
                    {
                        services.AddSingleton<IAuthorizationHandler, AllowAnonymousHandler>();
                        services.AddControllers(options =>
                        {
                            options.Filters.Add(new FakeUserFilter());
                        });
                    }

                });
            });

            return factory;
        }


    }
}
