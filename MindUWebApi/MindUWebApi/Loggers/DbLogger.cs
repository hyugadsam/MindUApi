using DBService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace MindUWebApi.Loggers
{
    public class DbLogger : ILogger
    {
        private readonly MindUContext _contextFactory;

        public DbLogger(MindUContext context)
        {
            this._contextFactory = context;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            //throw new NotImplementedException();
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            //throw new NotImplementedException();
            return logLevel != LogLevel.None;   //Se permite cualquier nivel de log excepto None
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;    //Valida que el nivel/tipo de log este habilitado

            Task.Run( async () =>
            {
                try
                {
                    //var log = new ApiLogs();

                    //_contextFactory.ApiLogs.Add(log);
                    //await _contextFactory.SaveChangesAsync();
                }
                catch (Exception)
                {

                }

            });

        }
    }
}
