using DBService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MindUWebApi.Loggers
{
    //Proveedor del logger, es el que se inyecta como dependencia y tiene acceso a los options
    public class DbLoggerProvider : ILoggerProvider
    {
        private readonly MindUContext context;

        public DbLoggerProvider(MindUContext context)
        {
            this.context = context;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new DbLogger(context); // This porque el contructor recibe el mismo objeto que lo esta creando
        }

        public void Dispose()
        {
            //throw new System.NotImplementedException();
            //Manejar la libreacion del provider y de la clase "" que tiene la logica de los logs
        }


    }
}
