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
    public class LevelsControllerTest: BasePruebas<DBService.Services.LevelsService>
    {
        [TestMethod]
        public async Task GetLevelsTest()
        {
            //Preparar
            var nombre = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombre);
            var mapper = ConfigureMapper();
            var logger = ConfigureLogger();

            context.Levels.AddRange(new List<DBService.Entities.Levels>() {
                new DBService.Entities.Levels { Description = "Jr" },
                new DBService.Entities.Levels { Description = "Mid" },
                new DBService.Entities.Levels { Description = "Mid++" },
                new DBService.Entities.Levels { Description = "Sr" },
            });
            await context.SaveChangesAsync();
            var context2 = ConstruirContext(nombre);

            //Prueba
            var controller = new LevelsController(new AppServiceLevels(context2, logger, mapper));
            var respuesta = await controller.GetList();

            //Verificar
            var levels = respuesta.Value;
            Assert.AreEqual(4, levels.Count);
            //Prueba
            var context3 = ConstruirContext(nombre);
            var DbLevels = await context3.Levels.ToListAsync();
            //Verificar

            Assert.AreEqual(levels.Count, DbLevels.Count);
        }

        [TestMethod]
        public async Task CreateLevelOkTest()
        {
            //Preparar
            var nombre = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombre);
            var mapper = ConfigureMapper();
            var logger = ConfigureLogger();

            //Prueba
            var controller = new LevelsController(new AppServiceLevels(context, logger, mapper));
            var respuesta = await controller.Create("Teach Lead");

            //Verificar
            var level = respuesta.Value;
            Assert.AreEqual(1, level.Id);

            //Prueba
            var context2 = ConstruirContext(nombre);
            var lista = await context.Levels.ToListAsync();

            //Verificar
            Assert.AreEqual(1, lista.Count);
        }

        [TestMethod]
        public async Task CreateLevelEmptyTest()
        {
            //Preparar
            var nombre = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombre);
            var mapper = ConfigureMapper();
            var logger = ConfigureLogger();

            //Prueba
            var controller = new LevelsController(new AppServiceLevels(context, logger, mapper));
            var respuesta = await controller.Create(String.Empty);

            //respuesta.Result.

            //Verificar
            var level = Converter.GetObjectResultContent<Dtos.Responses.BasicCreateResponse>(respuesta.Result);
            Assert.AreEqual(400, level.Code);

        }

        [TestMethod]
        public async Task UpdateLevelOkTest()
        {
            //Preparar
            var nombre = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombre);
            var mapper = ConfigureMapper();
            var logger = ConfigureLogger();

            context.Levels.Add(new DBService.Entities.Levels { Description = "Jrrrrr" });
            await context.SaveChangesAsync();

            //Prueba
            string NewDescription = "Jr";
            var context2 = ConstruirContext(nombre);
            var controller = new LevelsController(new AppServiceLevels(context2, logger, mapper));
            var respuesta = await controller.Update(new Dtos.Dtos.LevelDto { Description = NewDescription, LevelId = 1 });

            //Verificar
            var resp = respuesta.Value;
            Assert.AreEqual(200, resp.Code);

            //Prueba

            var context3 = ConstruirContext(nombre);
            var exists = await context3.Levels.AnyAsync(x => x.Description == NewDescription);

            //Verificar
            Assert.IsTrue(exists);

        }

        [TestMethod]
        public async Task UpdateLevelNoRowTest()
        {
            //Preparar
            var nombre = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombre);
            var mapper = ConfigureMapper();
            var logger = ConfigureLogger();

            //Prueba
            string NewDescription = "Jr";
            var controller = new LevelsController(new AppServiceLevels(context, logger, mapper));
            var respuesta = await controller.Update(new Dtos.Dtos.LevelDto { LevelId= 100, Description = NewDescription });

            //Verificar
            var resp = Converter.GetObjectResultContent<Dtos.Responses.BasicResponse>(respuesta.Result); ;
            Assert.AreEqual(400, resp.Code);
        }

        [TestMethod]
        public async Task DeleteLevelNoRowTest()
        {
            //Preparar
            var nombre = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombre);
            var mapper = ConfigureMapper();
            var logger = ConfigureLogger();

            //Prueba
            var controller = new LevelsController(new AppServiceLevels(context, logger, mapper));
            var respuesta = await controller.Delete(100);

            //Verificar
            var resp = Converter.GetObjectResultContent<Dtos.Responses.BasicResponse>(respuesta.Result);
            Assert.AreEqual(400, resp.Code);
        }

        [TestMethod]
        public async Task DeleteLevelOKTest()
        {
            //Preparar
            var nombre = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombre);
            var mapper = ConfigureMapper();
            var logger = ConfigureLogger();

            context.Levels.Add(new DBService.Entities.Levels { Description = "Jr" });
            await context.SaveChangesAsync();

            //Prueba
            var context2 = ConstruirContext(nombre);
            var controller = new LevelsController(new AppServiceLevels(context2, logger, mapper));
            var respuesta = await controller.Delete(1);

            //Verificar
            var resp = respuesta.Value;
            Assert.AreEqual(200, resp.Code);

            //Prueba
            var context3 = ConstruirContext(nombre);
            var exists = await context3.Levels.AnyAsync(x => x.Description == "Jr");

            //Verificar
            Assert.IsFalse(exists);

        }


    }
}
