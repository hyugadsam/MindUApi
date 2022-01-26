using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MindUWebApi.Test.Utilidades
{
    public class FakeUserFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            context.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new Claim("Name", "admin"),
                new Claim("Email", "admin@example.com"),
                new Claim("Role", "SuperAdmin")

            }, "Prueba"));

            await next();
        }

    }
}
