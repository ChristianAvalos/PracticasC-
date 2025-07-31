﻿using Microsoft.AspNetCore.Mvc.Filters;

namespace BivliotecaAPI.Utilidades
{
    public class MiFiltroDeAccion : IActionFilter
    {
        private readonly ILogger<MiFiltroDeAccion> logger;

        public MiFiltroDeAccion(ILogger<MiFiltroDeAccion> logger)
        {
            this.logger = logger;
        }

        //antes de la accion
        public void OnActionExecuting(ActionExecutingContext context)
        {
            logger.LogInformation("Antes de la accion");
        }
        //despues de la accion
        public void OnActionExecuted(ActionExecutedContext context)
        {
            logger.LogInformation("Despues de la accion");
        }

        
    }
}
