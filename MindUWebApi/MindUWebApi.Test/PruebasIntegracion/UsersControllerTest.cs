using DBService.Services;
using Dtos.Dtos;
using Dtos.Request;
using Dtos.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MindUWebApi.Test.PruebasIntegracion
{
    [TestClass]
    public class UsersControllerTest : BasePruebas<UserService>
    {
        private static readonly string url = "api/v1/Users";

        [TestMethod]
        public async Task LoginOKTest()
        {
            var nombre = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombre);

            context.Roles.AddRange(new List<DBService.Entities.Roles>()
            {
                new DBService.Entities.Roles{ RoleName = "SuperAdmin" },
                new DBService.Entities.Roles{ RoleName = "Admin" },
                new DBService.Entities.Roles{ RoleName = "User" }
            });

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
            //client.DefaultRequestHeaders.Add("Content-Type", "application/json");
            var request = new LoginRequest
            {
                Email = "admin@example.com",
                Password = "aaaaa",
            };
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var respuest = await client.PostAsync($"{url}/Login", httpContent);

            ////Prueba
            respuest.EnsureSuccessStatusCode();

            var response = JsonConvert.DeserializeObject<LoginResponse>(await respuest.Content.ReadAsStringAsync());
            Assert.AreEqual(200, response.Code);
            Assert.IsFalse(string.IsNullOrEmpty(response.Token));

        }

        [TestMethod]
        public async Task LoginFailNoExistsTest()
        {
            var nombre = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombre);

            await context.SaveChangesAsync();

            var factory = GetWebAppFactory(nombre);

            var client = factory.CreateClient();
            //client.DefaultRequestHeaders.Add("Content-Type", "application/json");
            var request = new LoginRequest
            {
                Email = "admin@example.com",
                Password = "aaaaa",
            };
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var respuest = await client.PostAsync($"{url}/Login", httpContent);
            var response = JsonConvert.DeserializeObject<LoginResponse>(await respuest.Content.ReadAsStringAsync());

            ////Prueba
            Assert.AreEqual("Bad Request", respuest.ReasonPhrase);
            Assert.AreEqual(400, response.Code);
            Assert.IsTrue(string.IsNullOrEmpty(response.Token));
            Assert.AreEqual(response.Message, "User dosent exist");
        }

        [TestMethod]
        public async Task LoginFailInvalidCredentialsTest()
        {
            var nombre = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombre);

            context.Roles.AddRange(new List<DBService.Entities.Roles>()
            {
                new DBService.Entities.Roles{ RoleName = "SuperAdmin" },
                new DBService.Entities.Roles{ RoleName = "Admin" },
                new DBService.Entities.Roles{ RoleName = "User" }
            });

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
            //client.DefaultRequestHeaders.Add("Content-Type", "application/json");
            var request = new LoginRequest
            {
                Email = "admin@example.com",
                Password = "a",
            };
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var respuest = await client.PostAsync($"{url}/Login", httpContent);

            ////Prueba
            var response = JsonConvert.DeserializeObject<LoginResponse>(await respuest.Content.ReadAsStringAsync());

            ////Prueba
            Assert.AreEqual("Bad Request", respuest.ReasonPhrase);
            Assert.AreEqual(400, response.Code);
            Assert.IsTrue(string.IsNullOrEmpty(response.Token));
            Assert.AreEqual(response.Message, "Invalid credentials");

        }

        [TestMethod]
        public async Task CreateUserFailUnAuthorizedTest()
        {
            var nombre = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombre);

            context.Roles.AddRange(new List<DBService.Entities.Roles>()
            {
                new DBService.Entities.Roles{ RoleName = "SuperAdmin" },
                new DBService.Entities.Roles{ RoleName = "Admin" },
                new DBService.Entities.Roles{ RoleName = "User" }
            });

            await context.SaveChangesAsync();

            var factory = GetWebAppFactory(nombre, false);

            var client = factory.CreateClient();
            var request = new NewUserRequest
            {
                Email = "a@q.com",
                Name = "Juan",
                Password = "Password123*",
                RoleId = Dtos.Enums.EnumRoles.Admin
            };
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var respuest = await client.PostAsync($"{url}/CreateUser", httpContent);

            ////Prueba
            Assert.AreEqual("Unauthorized", respuest.ReasonPhrase);

        }

        [TestMethod]
        public async Task CreateUserFailNameLengthTest()
        {
            var nombre = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombre);
            var token = await GetLoginToken(nombre);

            context.Roles.AddRange(new List<DBService.Entities.Roles>()
            {
                new DBService.Entities.Roles{ RoleName = "SuperAdmin" },
                new DBService.Entities.Roles{ RoleName = "Admin" },
                new DBService.Entities.Roles{ RoleName = "User" }
            });

            await context.SaveChangesAsync();

            var factory = GetWebAppFactory(nombre);

            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var request = JsonConvert.SerializeObject(new NewUserRequest
            {
                Email = "a@q.com",
                Name = "Juan",  //Must be of 5
                Password = "Password123*",
                RoleId = Dtos.Enums.EnumRoles.Admin
            });
            HttpContent httpContent = new StringContent(request, Encoding.UTF8, "application/json");

            var respuest = await client.PostAsync($"{url}/CreateUser", httpContent);

            var test = await respuest.Content.ReadAsStringAsync();
            Assert.IsTrue(test.Contains("The field Name must be a string or array type with a minimum length of"));

        }

        [TestMethod]
        public async Task CreateUserFailInvalidRoleTest()
        {
            var nombre = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombre);
            var token = await GetLoginToken(nombre);

            context.Roles.AddRange(new List<DBService.Entities.Roles>()
            {
                new DBService.Entities.Roles{ RoleName = "SuperAdmin" },
                new DBService.Entities.Roles{ RoleName = "Admin" },
                new DBService.Entities.Roles{ RoleName = "User" }
            });

            await context.SaveChangesAsync();

            var factory = GetWebAppFactory(nombre);

            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var request = JsonConvert.SerializeObject(new NewUserRequest
            {
                Email = "a@q.com",
                Name = "Juan",  //Must be of 5
                Password = "Password123*",
                RoleId = Dtos.Enums.EnumRoles.SuperAdmin
            });
            HttpContent httpContent = new StringContent(request, Encoding.UTF8, "application/json");

            var respuest = await client.PostAsync($"{url}/CreateUser", httpContent);

            var test = await respuest.Content.ReadAsStringAsync();
            Assert.IsTrue(test.Contains("The field RoleId"));

        }

        [TestMethod]
        public async Task CreateUserOKTest()
        {
            var nombre = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombre);
            var token = await GetLoginToken(nombre);

            context.Roles.AddRange(new List<DBService.Entities.Roles>()
            {
                new DBService.Entities.Roles{ RoleName = "SuperAdmin" },
                new DBService.Entities.Roles{ RoleName = "Admin" },
                new DBService.Entities.Roles{ RoleName = "User" }
            });

            await context.SaveChangesAsync();

            var factory = GetWebAppFactory(nombre, false);

            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var request = JsonConvert.SerializeObject(new NewUserRequest
            {
                Email = "a@q.com",
                Name = "Juan Juan",
                Password = "Password123*",
                RoleId = Dtos.Enums.EnumRoles.Admin
            });
            HttpContent httpContent = new StringContent(request, Encoding.UTF8, "application/json");

            var respuest = await client.PostAsync($"{url}/CreateUser", httpContent);
            var test = await respuest.Content.ReadAsStringAsync();
            ////Prueba
            respuest.EnsureSuccessStatusCode();

            var response = JsonConvert.DeserializeObject<BasicResponse>(await respuest.Content.ReadAsStringAsync());
            Assert.AreEqual(200, response.Code);

        }

        [TestMethod]
        public async Task CreateBasicUserOKTest()
        {
            var nombre = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombre);

            context.Roles.AddRange(new List<DBService.Entities.Roles>()
            {
                new DBService.Entities.Roles{ RoleName = "SuperAdmin" },
                new DBService.Entities.Roles{ RoleName = "Admin" },
                new DBService.Entities.Roles{ RoleName = "User" }
            });

            await context.SaveChangesAsync();

            var factory = GetWebAppFactory(nombre);

            var client = factory.CreateClient();
            var request = JsonConvert.SerializeObject(new NewBasicUserRequest
            {
                Email = "a@q.com",
                Name = "Juan Juan",
                Password = "Password123*",
            });
            HttpContent httpContent = new StringContent(request, Encoding.UTF8, "application/json");

            var respuest = await client.PostAsync($"{url}/CreateNormalUser", httpContent);
            ////Prueba
            respuest.EnsureSuccessStatusCode();

            var response = JsonConvert.DeserializeObject<BasicResponse>(await respuest.Content.ReadAsStringAsync());
            Assert.AreEqual(200, response.Code);

        }

        [TestMethod]
        public async Task CreateBasicUserFailPasswordLengthTest()
        {
            var nombre = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombre);

            context.Roles.AddRange(new List<DBService.Entities.Roles>()
            {
                new DBService.Entities.Roles{ RoleName = "SuperAdmin" },
                new DBService.Entities.Roles{ RoleName = "Admin" },
                new DBService.Entities.Roles{ RoleName = "User" }
            });

            await context.SaveChangesAsync();

            var factory = GetWebAppFactory(nombre);

            var client = factory.CreateClient();
            var request = JsonConvert.SerializeObject(new NewBasicUserRequest
            {
                Email = "a@q.com",
                Name = "Juan Juan",
                Password = "a",
            });
            HttpContent httpContent = new StringContent(request, Encoding.UTF8, "application/json");

            var respuest = await client.PostAsync($"{url}/CreateNormalUser", httpContent);
            var test = await respuest.Content.ReadAsStringAsync();
            ////Prueba
            Assert.AreEqual("Bad Request", respuest.ReasonPhrase);
            Assert.IsTrue(test.Contains("The field Password"));

        }

        [TestMethod]
        public async Task UpdateUserWithOutPasswordOKTest()
        {
            var nombre = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombre);
            var token = await GetLoginToken(nombre);
            context.Roles.AddRange(new List<DBService.Entities.Roles>()
            {
                new DBService.Entities.Roles{ RoleName = "SuperAdmin" },
                new DBService.Entities.Roles{ RoleName = "Admin" },
                new DBService.Entities.Roles{ RoleName = "User" }
            });
            context.Users.Add(new DBService.Entities.Users
            {
                Email = "a@q.com",
                Name = "a@q.com",
                IsActive = true,
                Password = "a",
                Salt = "a",
                RoleId = 1
            });

            await context.SaveChangesAsync();

            var factory = GetWebAppFactory(nombre);

            var client = factory.CreateClient();
            //client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var request = JsonConvert.SerializeObject(new UpdateUserRequest
            {
                Email = "a@q.com",
                Name = "Juan Juan",
                IsActive = false,
                RoleId = Dtos.Enums.EnumRoles.User
            });

            HttpContent httpContent = new StringContent(request, Encoding.UTF8, "application/json");
            var respuest = await client.PutAsync($"{url}/UpdateUser", httpContent);

            respuest.EnsureSuccessStatusCode();
            var response = JsonConvert.DeserializeObject<BasicResponse>(await respuest.Content.ReadAsStringAsync());
            ////Prueba
            Assert.AreEqual(200, response.Code);

            var context2 = ConstruirContext(nombre);
            var user = await context2.Users.Where(u => u.Email == "a@q.com").FirstOrDefaultAsync();
            Assert.AreEqual("a", user.Password);
            Assert.AreEqual(3, user.RoleId);
            Assert.AreEqual("a", user.Salt);
            Assert.AreEqual("Juan Juan", user.Name);

        }

        [TestMethod]
        public async Task UpdateUserWithPasswordOKTest()
        {
            var nombre = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombre);

            context.Roles.AddRange(new List<DBService.Entities.Roles>()
            {
                new DBService.Entities.Roles{ RoleName = "SuperAdmin" },
                new DBService.Entities.Roles{ RoleName = "Admin" },
                new DBService.Entities.Roles{ RoleName = "User" }
            });
            context.Users.Add(new DBService.Entities.Users
            {
                Email = "a@q.com",
                Name = "a@q.com",
                IsActive = true,
                Password = "a",
                Salt = "a",
                RoleId = 1
            });

            await context.SaveChangesAsync();

            var factory = GetWebAppFactory(nombre);

            var client = factory.CreateClient();
            var request = JsonConvert.SerializeObject(new UpdateUserRequest
            {
                Email = "a@q.com",
                Name = "Juan Juan",
                IsActive = false,
                RoleId = Dtos.Enums.EnumRoles.User,
                Password = "Password"
            });

            HttpContent httpContent = new StringContent(request, Encoding.UTF8, "application/json");
            var respuest = await client.PutAsync($"{url}/UpdateUser", httpContent);

            respuest.EnsureSuccessStatusCode();
            var response = JsonConvert.DeserializeObject<BasicResponse>(await respuest.Content.ReadAsStringAsync());
            ////Prueba
            Assert.AreEqual(200, response.Code);

            var context2 = ConstruirContext(nombre);
            var user = await context2.Users.Where(u => u.Email == "a@q.com").FirstOrDefaultAsync();
            Assert.IsFalse(user.Password.Equals("a"));
            Assert.AreEqual(3, user.RoleId);
            Assert.IsFalse(user.Salt.Equals("a"));
            Assert.AreEqual("Juan Juan", user.Name);

        }

        [TestMethod]
        public async Task UpdateUserFailNoExistsTest()
        {
            var nombre = Guid.NewGuid().ToString();

            var factory = GetWebAppFactory(nombre);

            var client = factory.CreateClient();
            var request = JsonConvert.SerializeObject(new UpdateUserRequest
            {
                Email = "a@q.com",
                Name = "Juan Juan",
                IsActive = false,
                RoleId = Dtos.Enums.EnumRoles.User,
                Password = "Password"
            });

            HttpContent httpContent = new StringContent(request, Encoding.UTF8, "application/json");
            var respuest = await client.PutAsync($"{url}/UpdateUser", httpContent);
            var test = await respuest.Content.ReadAsStringAsync();

            Assert.AreEqual("Bad Request", respuest.ReasonPhrase);
            Assert.IsTrue(test.Contains("User dosent exist"));
        }

        [TestMethod]
        public async Task DeactivateUserFailNoExistsTest()
        {
            var nombre = Guid.NewGuid().ToString();

            var factory = GetWebAppFactory(nombre);

            var client = factory.CreateClient();
            var request = JsonConvert.SerializeObject("a@q.com");

            HttpContent httpContent = new StringContent(request, Encoding.UTF8, "application/json");
            var respuest = await client.PutAsync($"{url}/Deactivate", httpContent);
            var test = await respuest.Content.ReadAsStringAsync();

            Assert.AreEqual("Bad Request", respuest.ReasonPhrase);
            Assert.IsTrue(test.Contains("User dosent exist"));
        }


        [TestMethod]
        public async Task DeactivateUserOKTest()
        {
            var nombre = Guid.NewGuid().ToString();
            var factory = GetWebAppFactory(nombre);
            var context = ConstruirContext(nombre);

            context.Roles.AddRange(new List<DBService.Entities.Roles>()
            {
                new DBService.Entities.Roles{ RoleName = "SuperAdmin" },
                new DBService.Entities.Roles{ RoleName = "Admin" },
                new DBService.Entities.Roles{ RoleName = "User" }
            });
            context.Users.Add(new DBService.Entities.Users
            {
                Email = "a@q.com",
                Name = "a@q.com",
                IsActive = true,
                Password = "a",
                Salt = "a",
                RoleId = 1
            });

            await context.SaveChangesAsync();

            var client = factory.CreateClient();
            var request = JsonConvert.SerializeObject("a@q.com");

            HttpContent httpContent = new StringContent(request, Encoding.UTF8, "application/json");
            var respuest = await client.PutAsync($"{url}/Deactivate", httpContent);
            respuest.EnsureSuccessStatusCode();
            var test = JsonConvert.DeserializeObject<BasicResponse>(await respuest.Content.ReadAsStringAsync());

            Assert.AreEqual(200, test.Code);
            var context2 = ConstruirContext(nombre);
            var user = await context2.Users.Where(u => u.Email == "a@q.com").FirstOrDefaultAsync();
            Assert.IsFalse(user.IsActive);
        }

        [TestMethod]
        public async Task GetUsersOKTest()
        {
            var nombre = Guid.NewGuid().ToString();
            var factory = GetWebAppFactory(nombre);
            var context = ConstruirContext(nombre);

            context.Roles.AddRange(new List<DBService.Entities.Roles>()
            {
                new DBService.Entities.Roles{ RoleName = "SuperAdmin" },
                new DBService.Entities.Roles{ RoleName = "Admin" },
                new DBService.Entities.Roles{ RoleName = "User" }
            });
            context.Users.Add(new DBService.Entities.Users
            {
                Email = "a@q.com",
                Name = "a@q.com",
                IsActive = true,
                Password = "a",
                Salt = "a",
                RoleId = 1
            });

            await context.SaveChangesAsync();

            var client = factory.CreateClient();

            var respuest = await client.GetAsync($"{url}/GetAll");
            respuest.EnsureSuccessStatusCode();
            var test = JsonConvert.DeserializeObject<List<UserDto>>(await respuest.Content.ReadAsStringAsync());

            Assert.IsTrue( test.Count > 0);
        }

        [TestMethod]
        public async Task GetUsersNoRowsTest()
        {
            var nombre = Guid.NewGuid().ToString();
            var factory = GetWebAppFactory(nombre);

            var client = factory.CreateClient();
            var respuest = await client.GetAsync($"{url}/GetAll");
            respuest.EnsureSuccessStatusCode();
            var test = JsonConvert.DeserializeObject<List<UserDto>>(await respuest.Content.ReadAsStringAsync());

            Assert.AreEqual(0, test.Count);
        }

    }

}
