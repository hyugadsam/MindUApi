using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MindUWebApi.Test.Utilidades
{
    public class AllowAnonymousHandler : IAuthorizationHandler
    {
        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            foreach (var item in context.PendingRequirements.ToList())
            {
                context.Succeed(item);
            }
            return Task.CompletedTask;
        }

    }

}
