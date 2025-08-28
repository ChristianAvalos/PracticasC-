using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BivliotecaAPI.Utilidades
{
    public class HATEOASFilterAttribute: ResultFilterAttribute

    {
        protected bool DebeincluirHATEOAS(ResultExecutingContext context)
        {
            if (context.Result is not ObjectResult result || !EsRespuestaExitosa(result))
            {                 
                return false;
            }
            if (!context.HttpContext.Request.Headers.TryGetValue("IncluirHATEOS",
                out var cabecera))
            {
                return false;
            }
            return string.Equals(cabecera.ToString(), "Y",
                StringComparison.InvariantCultureIgnoreCase);

        }
        private bool EsRespuestaExitosa(ObjectResult result)
        {
            if (result.Value is null)
            {
                return false;    
            }
            if (result.StatusCode.HasValue && !result.StatusCode.Value.ToString().StartsWith("2"))
            {
                return false;
            }

            return true;

        }
    }
}
