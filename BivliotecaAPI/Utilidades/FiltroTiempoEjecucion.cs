using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace BivliotecaAPI.Utilidades
{
    public class FiltroTiempoEjecucion : IAsyncActionFilter
    {
        private readonly ILogger<FiltroTiempoEjecucion> logger;

        public FiltroTiempoEjecucion(ILogger<FiltroTiempoEjecucion> logger)
        {
            this.logger = logger;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Antes de la ejecucion de la accion
            var stopwatch = Stopwatch.StartNew();
            logger.LogInformation($"Inicio de la accion: { context.ActionDescriptor.DisplayName}");

            await next();

            // Despues de la ejecucion de la accion
            stopwatch.Stop();
            logger.LogInformation($"Fin de la ejecucion de la accion: {context.ActionDescriptor.DisplayName} - Tiempo transcurrido: {stopwatch.ElapsedMilliseconds} ms");
        }
    }
}
