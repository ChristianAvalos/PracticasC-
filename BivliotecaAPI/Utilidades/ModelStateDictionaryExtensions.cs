using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BivliotecaAPI.Utilidades
{
    public static class ModelStateDictionaryExtensions
    {
        public static BadRequestObjectResult ConstruirPorblemaDetail(this ModelStateDictionary modelState)
        {
            var problemDetails = new ValidationProblemDetails(modelState)
            {
                Type = "https://biblioteca.com/modelstateerror",
                Title = "Errores de validacion",
                Status = StatusCodes.Status400BadRequest,
                Detail = "Por favor revise los errores y vuelva a intentarlo.",
                Instance = string.Empty
            };
            return new BadRequestObjectResult(problemDetails);
        }
    }
}
