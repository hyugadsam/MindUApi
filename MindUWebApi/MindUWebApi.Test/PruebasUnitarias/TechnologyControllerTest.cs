using ApplicationServices.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindUWebApi.Controllers.V1;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MindUWebApi.Test.PruebasUnitarias
{
    [TestClass]
    public class TechnologyControllerTest: BasePruebas<DBService.Services.TechnologiesService>
    {
        [TestMethod]
        public async Task GetTechnologiesTest()
        {
            //Preparar
            var nombre = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombre);
            var mapper = ConfigureMapper();
            var logger = ConfigureLogger();

            context.Technologies.AddRange(new List<DBService.Entities.Technologies>() {
                new DBService.Entities.Technologies { Description = ".Net Core" },
                new DBService.Entities.Technologies { Description = ".Net Framework" },
            });
            await context.SaveChangesAsync();
            var context2 = ConstruirContext(nombre);

            //Prueba
            var controller = new TechnologiesController(new AppServiceTechnologies(context2, logger, mapper));
            var respuesta = await controller.GetList();

            //Verificar
            var technologies = respuesta.Value;
            Assert.AreEqual(2, technologies.Count);
            //Prueba
            var context3 = ConstruirContext(nombre);
            var DbTechnologies = await context3.Technologies.ToListAsync();
            //Verificar

            Assert.AreEqual(technologies.Count, DbTechnologies.Count);
        }

        [TestMethod]
        public async Task CreateTechnologyOkTest()
        {
            //Preparar
            var nombre = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombre);
            var mapper = ConfigureMapper();
            var logger = ConfigureLogger();

            //Prueba
            var controller = new TechnologiesController(new AppServiceTechnologies(context, logger, mapper));
            var respuesta = await controller.CreateTechnology(".Net Core");

            //Verificar
            var technology = respuesta.Value;
            Assert.AreEqual(1, technology.Id);

            //Prueba
            var context2 = ConstruirContext(nombre);
            var lista = await context.Technologies.ToListAsync();

            //Verificar
            Assert.AreEqual(1, lista.Count);
        }

        [TestMethod]
        public async Task CreateTechnologyEmptyTest()
        {
            //Preparar
            var nombre = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombre);
            var mapper = ConfigureMapper();
            var logger = ConfigureLogger();

            //Prueba
            var controller = new TechnologiesController(new AppServiceTechnologies(context, logger, mapper));
            var respuesta = await controller.CreateTechnology(String.Empty);

            //respuesta.Result.

            //Verificar
            var Technology = Converter.GetObjectResultContent<Dtos.Responses.BasicCreateResponse>(respuesta.Result);
            Assert.AreEqual(400, Technology.Code);

        }

        [TestMethod]
        public async Task UpdateTechnologyOkTest()
        {
            //Preparar
            var nombre = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombre);
            var mapper = ConfigureMapper();
            var logger = ConfigureLogger();

            context.Technologies.Add(new DBService.Entities.Technologies { Description = ".net" });
            await context.SaveChangesAsync();

            //Prueba
            string NewDescription = ".Net Core 6.0";
            var context2 = ConstruirContext(nombre);
            var controller = new TechnologiesController(new AppServiceTechnologies(context2, logger, mapper));
            var respuesta = await controller.UpdateTechnology(new Dtos.Dtos.TechnologyDto  { Description = NewDescription, TechnologyId = 1 });

            //Verificar
            var resp = respuesta.Value;
            Assert.AreEqual(200, resp.Code);

            //Prueba

            var context3 = ConstruirContext(nombre);
            var exists = await context3.Technologies.AnyAsync(x => x.Description == NewDescription);

            //Verificar
            Assert.IsTrue(exists);

        }

        [TestMethod]
        public async Task UpdateTechnologyNoRowTest()
        {
            //Preparar
            var nombre = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombre);
            var mapper = ConfigureMapper();
            var logger = ConfigureLogger();

            //Prueba
            string NewDescription = ".net";
            var controller = new TechnologiesController(new AppServiceTechnologies(context, logger, mapper));
            var respuesta = await controller.UpdateTechnology(new Dtos.Dtos.TechnologyDto { TechnologyId = 1000, Description = NewDescription });

            //Verificar
            var resp = Converter.GetObjectResultContent<Dtos.Responses.BasicResponse>(respuesta.Result);
            Assert.AreEqual(400, resp.Code);
        }

        [TestMethod]
        public async Task DeleteTechnologyNoRowTest()
        {
            //Preparar
            var nombre = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombre);
            var mapper = ConfigureMapper();
            var logger = ConfigureLogger();

            //Prueba
            var controller = new TechnologiesController(new AppServiceTechnologies(context, logger, mapper));
            var respuesta = await controller.Delete(1000);

            //Verificar
            var resp = Converter.GetObjectResultContent<Dtos.Responses.BasicResponse>(respuesta.Result);
            Assert.AreEqual(400, resp.Code);
        }

        [TestMethod]
        public async Task DeleteTechnologyOKTest()
        {
            //Preparar
            var nombre = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombre);
            var mapper = ConfigureMapper();
            var logger = ConfigureLogger();

            context.Technologies.Add(new DBService.Entities.Technologies { Description = "java" });
            await context.SaveChangesAsync();

            //Prueba
            var context2 = ConstruirContext(nombre);
            var controller = new TechnologiesController(new AppServiceTechnologies(context2, logger, mapper));
            var respuesta = await controller.Delete(1);

            //Verificar
            var resp = respuesta.Value;
            Assert.AreEqual(200, resp.Code);

            //Prueba
            var context3 = ConstruirContext(nombre);
            var exists = await context3.Technologies.AnyAsync(x => x.Description == "java");

            //Verificar
            Assert.IsFalse(exists);

        }


    }
}
