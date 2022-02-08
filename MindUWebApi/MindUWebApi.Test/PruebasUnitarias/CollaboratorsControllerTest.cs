using ApplicationServices.Services;
using DBService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindUWebApi.Controllers.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindUWebApi.Test.PruebasUnitarias
{
    [TestClass]
    public class CollaboratorsControllerTest : BasePruebas<DBService.Services.CollaboratorsService>
    {

        [TestMethod]
        public async Task ObtenerColaboradores()
        {
            //Preparar
            #region Preparar
            var nombre = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombre);
            var mapper = ConfigureMapper();
            var logger = ConfigureLogger();
            context.Technologies.AddRange(new List<Technologies>()  //Agregar tecnologias
            {
                new Technologies{ Description = ".net Core" },
                new Technologies{ Description = ".net Framework" },
            });
            await context.SaveChangesAsync();

            context.Levels.Add(new Levels { Description = "Mid" }); //Agregar niveles
            context.Levels.Add(new Levels { Description = "Mid++" });
            await context.SaveChangesAsync();

            context.Collaborators.AddRange(new List<Collaborators>() {  //Agregar colaborador
                new Collaborators
                {
                    FullName = "Juan",
                    IsActive = true,
                    IsGraduated = false,
                    Levelid = 1,
                    TimeZone = TimeZoneInfo.Local.ToString(),
                    CollaboratorsTechnologies = new List<CollaboratorsTechnologies>()
                    {
                        new CollaboratorsTechnologies()
                        {
                            Certificates = String.Empty,
                            ProyectsUrl = String.Empty,
                            TechnologyId = 1,
                            YearsExperience = 1,
                        },
                        new CollaboratorsTechnologies()
                        {
                            Certificates = String.Empty,
                            ProyectsUrl = String.Empty,
                            TechnologyId = 2,
                            YearsExperience = 2,
                        }
                    }
                },
                new Collaborators
                {
                    FullName = "Jose",
                    IsActive = true,
                    IsGraduated = false,
                    Levelid = 2,
                    TimeZone = TimeZoneInfo.Local.ToString(),
                    CollaboratorsTechnologies = null
                }
            });

            await context.SaveChangesAsync();
            var context2 = ConstruirContext(nombre);

            #endregion

            //Prueba
            var controller = new CollaboratorsController(new AppServiceCollaborators(context2, logger, mapper));
            var respuesta = await controller.GetList(new Dtos.Dtos.PaginacionDTO());

            //Verificar
            var collaborators = respuesta.Value;
            Assert.AreEqual(2, collaborators.Count);
        }

        [TestMethod]
        public async Task ObtenerColaboradorPorId()
        {
            //Preparar
            #region Preparar
            var nombre = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombre);
            var mapper = ConfigureMapper();
            var logger = ConfigureLogger();
            context.Technologies.AddRange(new List<Technologies>()  //Agregar tecnologias
            {
                new Technologies{ Description = ".net Core" },
                new Technologies{ Description = ".net Framework" },
            });
            await context.SaveChangesAsync();

            context.Levels.Add(new Levels { Description = "Mid" }); //Agregar niveles
            context.Levels.Add(new Levels { Description = "Mid++" });
            await context.SaveChangesAsync();

            context.Collaborators.AddRange(new List<Collaborators>() {  //Agregar colaborador
                new Collaborators
                {
                    FullName = "Juan",
                    IsActive = true,
                    IsGraduated = false,
                    Levelid = 1,
                    TimeZone = TimeZoneInfo.Local.ToString(),
                    CollaboratorsTechnologies = new List<CollaboratorsTechnologies>()
                    {
                        new CollaboratorsTechnologies()
                        {
                            Certificates = String.Empty,
                            ProyectsUrl = String.Empty,
                            TechnologyId = 1,
                            YearsExperience = 1,
                        },
                        new CollaboratorsTechnologies()
                        {
                            Certificates = String.Empty,
                            ProyectsUrl = String.Empty,
                            TechnologyId = 2,
                            YearsExperience = 2,
                        }
                    }
                }
            });

            await context.SaveChangesAsync();
            var context2 = ConstruirContext(nombre);

            #endregion

            //Prueba
            var controller = new CollaboratorsController(new AppServiceCollaborators(context2, logger, mapper));
            var respuesta = await controller.Get(1);

            //Verificar
            var collaborator = respuesta.Value;
            Assert.AreEqual(1, collaborator.CollaboratorId);
            Assert.IsTrue(collaborator.Technologies.Count == 2);
        }

        [TestMethod]
        public async Task ObtenerColaboradorPorIdNoEncontrado()
        {
            //Preparar
            var nombre = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombre);
            var mapper = ConfigureMapper();
            var logger = ConfigureLogger();
            
            //Prueba
            var controller = new CollaboratorsController(new AppServiceCollaborators(context, logger, mapper));
            var respuesta = await controller.Get(123456);

            //Verificar
            Assert.AreEqual(null, respuesta.Value);
            Assert.AreEqual(null, respuesta.Result);
        }

        [TestMethod]
        public async Task CreateCollaboratorOkTest()
        {
            #region Preparar
            var nombre = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombre);
            var mapper = ConfigureMapper();
            var logger = ConfigureLogger();
            context.Technologies.AddRange(new List<Technologies>()  //Agregar tecnologias
            {
                new Technologies{ Description = ".net Core" },
                new Technologies{ Description = ".net Framework" },
            });
            await context.SaveChangesAsync();

            context.Levels.Add(new Levels { Description = "Mid" }); //Agregar niveles
            await context.SaveChangesAsync();
            #endregion

            //Prueba
            var context2 = ConstruirContext(nombre);
            var controller = new CollaboratorsController(new AppServiceCollaborators(context2, logger, mapper));
            var request = new Dtos.Request.NewCollaboratorRequest
            {
                FullName = "Juan",
                Levelid = 1,
                TimeZone = TimeZoneInfo.Local.ToString(),
                Technologies = new List<Dtos.Dtos.CollaboratorTechnologiesDto>
                {
                    new Dtos.Dtos.CollaboratorTechnologiesDto
                    {
                        Certificates = new List<string>{ "Certificado 1", "Certificado 2", "Certificado 3", "Certificado 4", },
                        ProyectsUrl = new List<string>{ "https://www.youtube.com", "https://www.google.com/" },
                        TechnologyId = 1,
                        YearsExperience = 5,
                    },
                    new Dtos.Dtos.CollaboratorTechnologiesDto
                    {
                        Certificates = new List<string>{ "Certificado 1", "Certificado 2", "Certificado 3", "Certificado 4", },
                        ProyectsUrl = new List<string>{ "https://www.youtube.com/2", "https://www.google.com/2" },
                        TechnologyId = 2,
                        YearsExperience = 5,
                    }
                }
            };
            var respuesta = await controller.Create(request);
            //Verificar
            var collaborator = respuesta.Value;
            Assert.AreEqual(1, collaborator.Id);
            //Prueba
            var context3 = ConstruirContext(nombre);
            var row = await context3.Collaborators.Include(ct => ct.CollaboratorsTechnologies).Where(c => c.CollaboratorId == collaborator.Id).FirstOrDefaultAsync();
            Assert.IsTrue(row != null);
            Assert.AreEqual(2, row.CollaboratorsTechnologies.Count);
            Assert.IsTrue(!string.IsNullOrEmpty(row.CollaboratorsTechnologies.First().Certificates));//Son xml, se parsean a lista en el controller
            Assert.IsTrue(!string.IsNullOrEmpty(row.CollaboratorsTechnologies.First().ProyectsUrl));//Son xml, se parsean a lista en el controller
        }

        [TestMethod]
        public async Task CreateCollaboratorOkNoTechnologiesTest()
        {
            var nombre = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombre);
            var mapper = ConfigureMapper();
            var logger = ConfigureLogger();

            context.Levels.Add(new Levels { Description = "Mid" }); //Agregar niveles
            await context.SaveChangesAsync();

            //Prueba
            var context2 = ConstruirContext(nombre);
            var controller = new CollaboratorsController(new AppServiceCollaborators(context2, logger, mapper));
            var request = new Dtos.Request.NewCollaboratorRequest
            {
                FullName = "Juan",
                Levelid = 1,
                TimeZone = TimeZoneInfo.Local.ToString(),
                Technologies = null
            };

            var respuesta = await controller.Create(request);
            //Verificar
            var collaborator = respuesta.Value;
            Assert.AreEqual(1, collaborator.Id);
            //Prueba
            var context3 = ConstruirContext(nombre);
            var row = await context3.Collaborators.Include(ct => ct.CollaboratorsTechnologies).Where(c => c.CollaboratorId == collaborator.Id).FirstOrDefaultAsync();
            Assert.IsTrue(row.FullName.Equals("Juan"));

        }

        [TestMethod]
        public async Task CreateCollaboratorFailLevelTest()
        {
            var nombre = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombre);
            var mapper = ConfigureMapper();
            var logger = ConfigureLogger();

            context.Levels.Add(new Levels { Description = "Mid" }); //Agregar niveles
            await context.SaveChangesAsync();

            //Prueba
            var context2 = ConstruirContext(nombre);
            var controller = new CollaboratorsController(new AppServiceCollaborators(context2, logger, mapper));
            var request = new Dtos.Request.NewCollaboratorRequest
            {
                FullName = "Juan",
                Levelid = 10000,
                TimeZone = TimeZoneInfo.Local.ToString(),
                Technologies = null
            };

            var respuesta = await controller.Create(request);
            //Verificar
            var response = Converter.GetObjectResultContent<Dtos.Responses.BasicCreateResponse>(respuesta.Result);
            Assert.AreEqual(400, response.Code);
            Assert.IsTrue(response.Message.Contains("El Levelid 10000 no existe"));
        }

        [TestMethod]
        public async Task CreateCollaboratorFailTechnologiesTest()
        {
            var nombre = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombre);
            var mapper = ConfigureMapper();
            var logger = ConfigureLogger();

            context.Levels.Add(new Levels { Description = "Mid" }); //Agregar niveles
            await context.SaveChangesAsync();

            //Prueba
            var context2 = ConstruirContext(nombre);
            var controller = new CollaboratorsController(new AppServiceCollaborators(context2, logger, mapper));
            var request = new Dtos.Request.NewCollaboratorRequest
            {
                FullName = "Juan",
                Levelid = 1,
                TimeZone = TimeZoneInfo.Local.ToString(),
                Technologies = new List<Dtos.Dtos.CollaboratorTechnologiesDto>
                {
                    new Dtos.Dtos.CollaboratorTechnologiesDto
                    {
                        Certificates = new List<string>{ "Certificado 1", "Certificado 2", "Certificado 3", "Certificado 4", },
                        ProyectsUrl = new List<string>{ "https://www.youtube.com", "https://www.google.com/" },
                        TechnologyId = 66666,
                        YearsExperience = 5,
                    },
                    new Dtos.Dtos.CollaboratorTechnologiesDto
                    {
                        Certificates = new List<string>{ "Certificado 1", "Certificado 2", "Certificado 3", "Certificado 4", },
                        ProyectsUrl = new List<string>{ "https://www.youtube.com/2", "https://www.google.com/2" },
                        TechnologyId = 77777,
                        YearsExperience = 5,
                    }
                }
            };

            var respuesta = await controller.Create(request);
            //Verificar
            var response = Converter.GetObjectResultContent<Dtos.Responses.BasicCreateResponse>(respuesta.Result);
            Assert.AreEqual(400, response.Code);
            Assert.IsTrue(response.Message.Contains($"El TechnologyId {66666} no existe"));
            Assert.IsTrue(response.Message.Contains($"El TechnologyId {77777} no existe"));
        }

        [TestMethod]
        public async Task UpdateCollaboratorOkTest()
        {
            #region Preparar
            var nombre = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombre);
            var mapper = ConfigureMapper();
            var logger = ConfigureLogger();
            context.Technologies.AddRange(new List<Technologies>()  //Agregar tecnologias
            {
                new Technologies{ Description = ".net Core" },
                new Technologies{ Description = ".net Framework" },
            });
            await context.SaveChangesAsync();

            context.Levels.Add(new Levels { Description = "Jr" }); //Agregar niveles
            context.Levels.Add(new Levels { Description = "Mid" }); //Agregar niveles
            await context.SaveChangesAsync();

            context.Collaborators.Add(new Collaborators
            {
                FullName = "Juan",
                IsActive = true,
                IsGraduated = false,
                Levelid = 1,
                TimeZone = TimeZoneInfo.Local.ToString()
            });
            await context.SaveChangesAsync();

            #endregion

            //Prueba
            var context2 = ConstruirContext(nombre);
            var controller = new CollaboratorsController(new AppServiceCollaborators(context2, logger, mapper));
            var request = new Dtos.Request.UpdateCollaboratorRequest
            {
                FullName = "Juan Juan Juan Juan",
                Levelid = 2,
                TimeZone = TimeZoneInfo.Utc.ToString(),
                CollaboratorId = 1,
                IsActive = false,
                Technologies = new List<Dtos.Dtos.CollaboratorTechnologiesDto>
                {
                    new Dtos.Dtos.CollaboratorTechnologiesDto
                    {
                        Certificates = new List<string>{ "Certificado 1", "Certificado 2", "Certificado 3", "Certificado 4", },
                        ProyectsUrl = new List<string>{ "https://www.youtube.com", "https://www.google.com/" },
                        TechnologyId = 1,
                        YearsExperience = 5,
                    },
                    new Dtos.Dtos.CollaboratorTechnologiesDto
                    {
                        Certificates = new List<string>{ "Certificado 1", "Certificado 2", "Certificado 3", "Certificado 4", },
                        ProyectsUrl = new List<string>{ "https://www.youtube.com/2", "https://www.google.com/2" },
                        TechnologyId = 2,
                        YearsExperience = 5,
                    }
                }
            };
            var respuesta = await controller.Update(request);
            //Verificar
            var resp = respuesta.Value;
            Assert.AreEqual(200, resp.Code);
            //Prueba
            var context3 = ConstruirContext(nombre);
            var row = await context3.Collaborators.Include(ct => ct.CollaboratorsTechnologies).Where(c => c.CollaboratorId == 1).FirstOrDefaultAsync();

            Assert.AreEqual("Juan Juan Juan Juan", row.FullName);
            Assert.AreEqual(2, row.Levelid);
            Assert.IsFalse(row.IsActive);
            Assert.AreEqual(2, row.CollaboratorsTechnologies.Count);
            Assert.IsTrue(!string.IsNullOrEmpty(row.CollaboratorsTechnologies.First().Certificates));//Son xml, se parsean a lista en el controller
            Assert.IsTrue(!string.IsNullOrEmpty(row.CollaboratorsTechnologies.First().ProyectsUrl));//Son xml, se parsean a lista en el controller
        }

        [TestMethod]
        public async Task UpdateCollaboratorOkNoTechnologiesTest()
        {
            #region Preparar
            var nombre = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombre);
            var mapper = ConfigureMapper();
            var logger = ConfigureLogger();
            context.Technologies.AddRange(new List<Technologies>()  //Agregar tecnologias
            {
                new Technologies{ Description = ".net Core" },
                new Technologies{ Description = ".net Framework" },
            });
            await context.SaveChangesAsync();

            context.Levels.Add(new Levels { Description = "Jr" }); //Agregar niveles
            context.Levels.Add(new Levels { Description = "Mid" }); //Agregar niveles
            await context.SaveChangesAsync();

            context.Collaborators.Add(new Collaborators
            {
                FullName = "Juan",
                IsActive = true,
                IsGraduated = false,
                Levelid = 1,
                TimeZone = TimeZoneInfo.Local.ToString()
            });
            await context.SaveChangesAsync();
            #endregion

            //Prueba
            var context2 = ConstruirContext(nombre);
            var controller = new CollaboratorsController(new AppServiceCollaborators(context2, logger, mapper));
            var request = new Dtos.Request.UpdateCollaboratorRequest
            {
                FullName = "Juan Juan Juan Juan",
                Levelid = 2,
                TimeZone = TimeZoneInfo.Utc.ToString(),
                CollaboratorId = 1,
                IsActive = false,
                Technologies = null
            };
            var respuesta = await controller.Update(request);

            //Verificar
            var resp = respuesta.Value;
            Assert.AreEqual(200, resp.Code);
            //Prueba
            var context3 = ConstruirContext(nombre);
            var row = await context3.Collaborators.Where(c => c.CollaboratorId == 1).FirstOrDefaultAsync();

            Assert.AreEqual("Juan Juan Juan Juan", row.FullName);
            Assert.AreEqual(2, row.Levelid);
            Assert.IsFalse(row.IsActive);
        }

        [TestMethod]
        public async Task UpdateCollaboratorFailLevelTest()
        {
            #region Preparar
            var nombre = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombre);
            var mapper = ConfigureMapper();
            var logger = ConfigureLogger();
            context.Technologies.AddRange(new List<Technologies>()  //Agregar tecnologias
            {
                new Technologies{ Description = ".net Core" },
                new Technologies{ Description = ".net Framework" },
            });
            await context.SaveChangesAsync();

            context.Levels.Add(new Levels { Description = "Jr" }); //Agregar niveles
            context.Levels.Add(new Levels { Description = "Mid" }); //Agregar niveles
            await context.SaveChangesAsync();

            context.Collaborators.Add(new Collaborators
            {
                FullName = "Juan",
                IsActive = true,
                IsGraduated = false,
                Levelid = 1,
                TimeZone = TimeZoneInfo.Local.ToString()
            });
            await context.SaveChangesAsync();
            #endregion

            //Prueba
            var context2 = ConstruirContext(nombre);
            var controller = new CollaboratorsController(new AppServiceCollaborators(context2, logger, mapper));
            var request = new Dtos.Request.UpdateCollaboratorRequest
            {
                FullName = "Juan Juan Juan Juan",
                Levelid = 22233,
                TimeZone = TimeZoneInfo.Utc.ToString(),
                CollaboratorId = 1,
                IsActive = false,
                Technologies = null
            };
            var respuesta = await controller.Update(request);

            //Verificar
            var response = Converter.GetObjectResultContent<Dtos.Responses.BasicResponse>(respuesta.Result);
            Assert.AreEqual(400, response.Code);
            Assert.IsTrue(response.Message.Contains("El Levelid 22233 no existe"));
        }

        [TestMethod]
        public async Task UpdateCollaboratorFailTechnologiesTest()
        {
            #region Preparar
            var nombre = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombre);
            var mapper = ConfigureMapper();
            var logger = ConfigureLogger();
            context.Technologies.AddRange(new List<Technologies>()  //Agregar tecnologias
            {
                new Technologies{ Description = ".net Core" },
                new Technologies{ Description = ".net Framework" },
            });
            await context.SaveChangesAsync();

            context.Levels.Add(new Levels { Description = "Jr" }); //Agregar niveles
            context.Levels.Add(new Levels { Description = "Mid" }); //Agregar niveles
            await context.SaveChangesAsync();

            context.Collaborators.Add(new Collaborators
            {
                FullName = "Juan",
                IsActive = true,
                IsGraduated = false,
                Levelid = 1,
                TimeZone = TimeZoneInfo.Local.ToString()
            });
            await context.SaveChangesAsync();
            #endregion

            //Prueba
            var context2 = ConstruirContext(nombre);
            var controller = new CollaboratorsController(new AppServiceCollaborators(context2, logger, mapper));
            var request = new Dtos.Request.UpdateCollaboratorRequest
            {
                FullName = "Juan Juan Juan Juan",
                Levelid = 1,
                TimeZone = TimeZoneInfo.Utc.ToString(),
                CollaboratorId = 1,
                IsActive = false,
                Technologies = new List<Dtos.Dtos.CollaboratorTechnologiesDto>
                {
                    new Dtos.Dtos.CollaboratorTechnologiesDto
                    {
                        Certificates = new List<string>{ "Certificado 1", "Certificado 2", "Certificado 3", "Certificado 4", },
                        ProyectsUrl = new List<string>{ "https://www.youtube.com", "https://www.google.com/" },
                        TechnologyId = 66666,
                        YearsExperience = 5,
                    },
                    new Dtos.Dtos.CollaboratorTechnologiesDto
                    {
                        Certificates = new List<string>{ "Certificado 1", "Certificado 2", "Certificado 3", "Certificado 4", },
                        ProyectsUrl = new List<string>{ "https://www.youtube.com/2", "https://www.google.com/2" },
                        TechnologyId = 77777,
                        YearsExperience = 5,
                    }
                }
            };
            var respuesta = await controller.Update(request);

            //Verificar
            var response = Converter.GetObjectResultContent<Dtos.Responses.BasicResponse>(respuesta.Result);
            Assert.AreEqual(400, response.Code);
            Assert.IsTrue(response.Message.Contains($"El TechnologyId {66666} no existe"));
            Assert.IsTrue(response.Message.Contains($"El TechnologyId {77777} no existe"));

        }

        [TestMethod]
        public async Task DeleteOk()
        {
            //Preparar
            #region Preparar
            var nombre = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombre);
            var mapper = ConfigureMapper();
            var logger = ConfigureLogger();
            context.Technologies.AddRange(new List<Technologies>()  //Agregar tecnologias
            {
                new Technologies{ Description = ".net Core" },
                new Technologies{ Description = ".net Framework" },
            });
            await context.SaveChangesAsync();

            context.Levels.Add(new Levels { Description = "Mid" }); //Agregar niveles
            context.Levels.Add(new Levels { Description = "Mid++" });
            await context.SaveChangesAsync();

            context.Collaborators.AddRange(new List<Collaborators>() {  //Agregar colaborador
                new Collaborators
                {
                    FullName = "Juan",
                    IsActive = true,
                    IsGraduated = false,
                    Levelid = 1,
                    TimeZone = TimeZoneInfo.Local.ToString(),
                    CollaboratorsTechnologies = new List<CollaboratorsTechnologies>()
                    {
                        new CollaboratorsTechnologies()
                        {
                            Certificates = String.Empty,
                            ProyectsUrl = String.Empty,
                            TechnologyId = 1,
                            YearsExperience = 1,
                        },
                        new CollaboratorsTechnologies()
                        {
                            Certificates = String.Empty,
                            ProyectsUrl = String.Empty,
                            TechnologyId = 2,
                            YearsExperience = 2,
                        }
                    }
                },
                new Collaborators
                {
                    FullName = "Jose",
                    IsActive = true,
                    IsGraduated = false,
                    Levelid = 2,
                    TimeZone = TimeZoneInfo.Local.ToString(),
                    CollaboratorsTechnologies = null
                }
            });

            await context.SaveChangesAsync();

            #endregion

            //Prueba
            var context2 = ConstruirContext(nombre);
            var controller = new CollaboratorsController(new AppServiceCollaborators(context2, logger, mapper));
            var respuesta = await controller.Delete(1);

            //Verificar
            var resp = respuesta.Value;
            Assert.AreEqual(200, resp.Code);

            //Prueba
            var context3 = ConstruirContext(nombre);
            var count = await context3.Collaborators.CountAsync();
            Assert.AreEqual(1, count);

        }

        [TestMethod]
        public async Task DeleteFailNoExists()
        {
            //Preparar
            #region Preparar
            var nombre = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombre);
            var mapper = ConfigureMapper();
            var logger = ConfigureLogger();
            context.Technologies.AddRange(new List<Technologies>()  //Agregar tecnologias
            {
                new Technologies{ Description = ".net Core" },
                new Technologies{ Description = ".net Framework" },
            });
            await context.SaveChangesAsync();

            context.Levels.Add(new Levels { Description = "Mid" }); //Agregar niveles
            context.Levels.Add(new Levels { Description = "Mid++" });
            await context.SaveChangesAsync();

            context.Collaborators.AddRange(new List<Collaborators>() {  //Agregar colaborador
                new Collaborators
                {
                    FullName = "Juan",
                    IsActive = true,
                    IsGraduated = false,
                    Levelid = 1,
                    TimeZone = TimeZoneInfo.Local.ToString(),
                    CollaboratorsTechnologies = new List<CollaboratorsTechnologies>()
                    {
                        new CollaboratorsTechnologies()
                        {
                            Certificates = String.Empty,
                            ProyectsUrl = String.Empty,
                            TechnologyId = 1,
                            YearsExperience = 1,
                        },
                        new CollaboratorsTechnologies()
                        {
                            Certificates = String.Empty,
                            ProyectsUrl = String.Empty,
                            TechnologyId = 2,
                            YearsExperience = 2,
                        }
                    }
                },
                new Collaborators
                {
                    FullName = "Jose",
                    IsActive = true,
                    IsGraduated = false,
                    Levelid = 2,
                    TimeZone = TimeZoneInfo.Local.ToString(),
                    CollaboratorsTechnologies = null
                }
            });

            await context.SaveChangesAsync();

            #endregion

            //Prueba
            var context2 = ConstruirContext(nombre);
            var controller = new CollaboratorsController(new AppServiceCollaborators(context2, logger, mapper));
            var respuesta = await controller.Delete(100000);

            //Verificar
            var resp = Converter.GetObjectResultContent<Dtos.Responses.BasicResponse>(respuesta.Result);
            Assert.AreEqual(400, resp.Code);

        }

        [TestMethod]
        public async Task DeactivateOK()
        {
            //Preparar
            #region Preparar
            var nombre = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombre);
            var mapper = ConfigureMapper();
            var logger = ConfigureLogger();
            context.Technologies.AddRange(new List<Technologies>()  //Agregar tecnologias
            {
                new Technologies{ Description = ".net Core" },
                new Technologies{ Description = ".net Framework" },
            });
            await context.SaveChangesAsync();

            context.Levels.Add(new Levels { Description = "Mid" }); //Agregar niveles
            context.Levels.Add(new Levels { Description = "Mid++" });
            await context.SaveChangesAsync();

            context.Collaborators.AddRange(new List<Collaborators>() {  //Agregar colaborador
                new Collaborators
                {
                    FullName = "Juan",
                    IsActive = true,
                    IsGraduated = false,
                    Levelid = 1,
                    TimeZone = TimeZoneInfo.Local.ToString(),
                    CollaboratorsTechnologies = new List<CollaboratorsTechnologies>()
                    {
                        new CollaboratorsTechnologies()
                        {
                            Certificates = String.Empty,
                            ProyectsUrl = String.Empty,
                            TechnologyId = 1,
                            YearsExperience = 1,
                        },
                        new CollaboratorsTechnologies()
                        {
                            Certificates = String.Empty,
                            ProyectsUrl = String.Empty,
                            TechnologyId = 2,
                            YearsExperience = 2,
                        }
                    }
                }
            });

            await context.SaveChangesAsync();

            #endregion

            //Prueba
            var context2 = ConstruirContext(nombre);
            var controller = new CollaboratorsController(new AppServiceCollaborators(context2, logger, mapper));
            var respuesta = await controller.DeactivateCollaborator(1);

            //Verificar
            var resp = respuesta.Value;
            Assert.AreEqual(200, resp.Code);

            //Prueba
            var context3 = ConstruirContext(nombre);
            var row = await context3.Collaborators.FirstOrDefaultAsync();
            Assert.AreEqual(false, row.IsActive);
        }

        [TestMethod]
        public async Task DeactivateFailNoExists()
        {
            //Preparar
            #region Preparar
            var nombre = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombre);
            var mapper = ConfigureMapper();
            var logger = ConfigureLogger();
            context.Technologies.AddRange(new List<Technologies>()  //Agregar tecnologias
            {
                new Technologies{ Description = ".net Core" },
                new Technologies{ Description = ".net Framework" },
            });
            await context.SaveChangesAsync();

            context.Levels.Add(new Levels { Description = "Mid" }); //Agregar niveles
            context.Levels.Add(new Levels { Description = "Mid++" });
            await context.SaveChangesAsync();

            context.Collaborators.AddRange(new List<Collaborators>() {  //Agregar colaborador
                new Collaborators
                {
                    FullName = "Juan",
                    IsActive = true,
                    IsGraduated = false,
                    Levelid = 1,
                    TimeZone = TimeZoneInfo.Local.ToString(),
                    CollaboratorsTechnologies = new List<CollaboratorsTechnologies>()
                    {
                        new CollaboratorsTechnologies()
                        {
                            Certificates = String.Empty,
                            ProyectsUrl = String.Empty,
                            TechnologyId = 1,
                            YearsExperience = 1,
                        },
                        new CollaboratorsTechnologies()
                        {
                            Certificates = String.Empty,
                            ProyectsUrl = String.Empty,
                            TechnologyId = 2,
                            YearsExperience = 2,
                        }
                    }
                }
            });

            await context.SaveChangesAsync();

            #endregion

            //Prueba
            var context2 = ConstruirContext(nombre);
            var controller = new CollaboratorsController(new AppServiceCollaborators(context2, logger, mapper));
            var respuesta = await controller.DeactivateCollaborator(100000);

            //Verificar
            var resp = Converter.GetObjectResultContent<Dtos.Responses.BasicResponse>(respuesta.Result);
            Assert.AreEqual(400, resp.Code);
        }

        [TestMethod]
        public async Task GraduateOK()
        {
            //Preparar
            #region Preparar
            var nombre = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombre);
            var mapper = ConfigureMapper();
            var logger = ConfigureLogger();
            context.Technologies.AddRange(new List<Technologies>()  //Agregar tecnologias
            {
                new Technologies{ Description = ".net Core" },
                new Technologies{ Description = ".net Framework" },
            });
            await context.SaveChangesAsync();

            context.Levels.Add(new Levels { Description = "Mid" }); //Agregar niveles
            context.Levels.Add(new Levels { Description = "Mid++" });
            await context.SaveChangesAsync();

            context.Collaborators.AddRange(new List<Collaborators>() {  //Agregar colaborador
                new Collaborators
                {
                    FullName = "Juan",
                    IsActive = true,
                    IsGraduated = false,
                    Levelid = 1,
                    TimeZone = TimeZoneInfo.Local.ToString(),
                    CollaboratorsTechnologies = null
                }
            });

            await context.SaveChangesAsync();

            #endregion

            //Prueba
            var context2 = ConstruirContext(nombre);
            var controller = new CollaboratorsController(new AppServiceCollaborators(context2, logger, mapper));
            var respuesta = await controller.GraduateCollaborator(1);

            //Verificar
            var resp = respuesta.Value;
            Assert.AreEqual(200, resp.Code);

            //Prueba
            var context3 = ConstruirContext(nombre);
            var row = await context3.Collaborators.FirstOrDefaultAsync();
            Assert.IsTrue(row.IsGraduated);
        }

        [TestMethod]
        public async Task GraduateFailAlreadyGraduated()
        {
            //Preparar
            #region Preparar
            var nombre = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombre);
            var mapper = ConfigureMapper();
            var logger = ConfigureLogger();
            context.Technologies.AddRange(new List<Technologies>()  //Agregar tecnologias
            {
                new Technologies{ Description = ".net Core" },
                new Technologies{ Description = ".net Framework" },
            });
            await context.SaveChangesAsync();

            context.Levels.Add(new Levels { Description = "Mid" }); //Agregar niveles
            context.Levels.Add(new Levels { Description = "Mid++" });
            await context.SaveChangesAsync();

            context.Collaborators.AddRange(new List<Collaborators>() {  //Agregar colaborador
                new Collaborators
                {
                    FullName = "Juan",
                    IsActive = true,
                    IsGraduated = true,
                    Levelid = 1,
                    TimeZone = TimeZoneInfo.Local.ToString(),
                    CollaboratorsTechnologies = null
                }
            });

            await context.SaveChangesAsync();

            #endregion

            //Prueba
            var context2 = ConstruirContext(nombre);
            var controller = new CollaboratorsController(new AppServiceCollaborators(context2, logger, mapper));
            var respuesta = await controller.GraduateCollaborator(1);

            //Verificar
            var resp = Converter.GetObjectResultContent<Dtos.Responses.BasicResponse>(respuesta.Result);
            Assert.AreEqual(400, resp.Code);

        }

        [TestMethod]
        public async Task GraduateFailNoExists()
        {
            //Preparar
            #region Preparar
            var nombre = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombre);
            var mapper = ConfigureMapper();
            var logger = ConfigureLogger();
            context.Technologies.AddRange(new List<Technologies>()  //Agregar tecnologias
            {
                new Technologies{ Description = ".net Core" },
                new Technologies{ Description = ".net Framework" },
            });
            await context.SaveChangesAsync();

            context.Levels.Add(new Levels { Description = "Mid" }); //Agregar niveles
            context.Levels.Add(new Levels { Description = "Mid++" });
            await context.SaveChangesAsync();

            context.Collaborators.AddRange(new List<Collaborators>() {  //Agregar colaborador
                new Collaborators
                {
                    FullName = "Juan",
                    IsActive = true,
                    IsGraduated = true,
                    Levelid = 1,
                    TimeZone = TimeZoneInfo.Local.ToString(),
                    CollaboratorsTechnologies = null
                }
            });

            await context.SaveChangesAsync();

            #endregion

            //Prueba
            var context2 = ConstruirContext(nombre);
            var controller = new CollaboratorsController(new AppServiceCollaborators(context2, logger, mapper));
            var respuesta = await controller.GraduateCollaborator(100000);

            //Verificar
            var resp = Converter.GetObjectResultContent<Dtos.Responses.BasicResponse>(respuesta.Result);
            Assert.AreEqual(400, resp.Code);

        }

    }
}
