using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace MindUWebApi.Filtros
{
    public class FiltroExcepcion : ExceptionFilterAttribute
    {
        private readonly ILogger<FiltroExcepcion> logger;

        public FiltroExcepcion(ILogger<FiltroExcepcion> logger)
        {
            this.logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            logger.LogError(context.Exception, context.Exception.Message);
            base.OnException(context);
        }

    }
}
