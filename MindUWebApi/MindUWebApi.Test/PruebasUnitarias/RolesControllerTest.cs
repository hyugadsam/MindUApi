using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindUWebApi.Controllers.V1;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using Dtos.Dtos;
using System.Linq;

namespace MindUWebApi.Test.PruebasUnitarias
{
    [TestClass]
    public class RolesControllerTest: BasePruebas<DBService.Services.RolesService>
    {
        [TestMethod]
        public async Task ObetenerLosRoles()
        {
            //Preparar
            var nombre = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombre);
            var mapper = ConfigureMapper();
            var logger = ConfigureLogger();
            context.Roles.Add(new DBService.Entities.Roles() { RoleName = "Role 1" });
            context.Roles.Add(new DBService.Entities.Roles() { RoleName = "Role 2" });
            context.Roles.Add(new DBService.Entities.Roles() { RoleName = "Role 3" });
            await context.SaveChangesAsync();

            var context2 = ConstruirContext(nombre);
            
            //Prueba
            var controller = new RolesController(new ApplicationServices.Services.AppServiceRoles(context2, mapper, logger));
            var respuesta = await controller.Get4();

            //Verificar
            var roles = respuesta.Value;
            Assert.AreEqual(2, roles.Count);    //2 porque se omite el rol con el id 1
            Assert.IsTrue(!roles.Any(roles => roles.RoleId == 1));
        }



    }
}
