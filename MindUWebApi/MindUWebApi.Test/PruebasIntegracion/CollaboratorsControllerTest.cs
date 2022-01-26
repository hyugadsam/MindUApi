using DBService.Services;
using Dtos.Responses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MindUWebApi.Test.PruebasIntegracion
{
    [TestClass]
    public class CollaboratorsControllerTest : BasePruebas<CollaboratorsService>
    {
        private static readonly string url = "api/v1/Collaborators";

        [TestMethod]
        public async Task GraduateCollaborator()
        {
            var nombre = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombre);

            context.Levels.Add(new DBService.Entities.Levels
            {
                Description = "MID"
            });
            await context.SaveChangesAsync();

            context.Collaborators.Add(new DBService.Entities.Collaborators
            {
                FullName = nombre,
                IsActive = true,
                IsGraduated = false,
                Levelid = 1,
                TimeZone = TimeZoneInfo.Local.ToString(),
            });
            
            await context.SaveChangesAsync();

            var factory = GetWebAppFactory(nombre);

            var client = factory.CreateClient();

            int CollaboratorId = 1;
            HttpContent httpContent = new StringContent(string.Empty, Encoding.UTF8, "application/json");
            var respuest = await client.PutAsync($"{url}/Graduate/{CollaboratorId}", httpContent);

            ////Prueba
            respuest.EnsureSuccessStatusCode();

            var response = JsonConvert.DeserializeObject<BasicResponse>(await respuest.Content.ReadAsStringAsync());
            Assert.AreEqual(200, response.Code);

        }


    }
}
