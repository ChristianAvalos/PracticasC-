using BivliotecaAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using System;

namespace BivliotecaAPI.Servicios.V1
{
    public class GeneradorEnlaces : IGeneradorEnlaces
    {
        private readonly LinkGenerator linkGenerator;
        private readonly IAuthorizationService authorizationService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public GeneradorEnlaces(LinkGenerator linkGenerator,
            IAuthorizationService authorizationService,
            IHttpContextAccessor httpContextAccessor)
        {
            this.linkGenerator = linkGenerator;
            this.authorizationService = authorizationService;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task GenerarEnlaces(AutorDTO autorDTO)
        {
            var usuario = httpContextAccessor.HttpContext!.User;
            var esAdmin = await authorizationService.AuthorizeAsync(usuario, "esAdmin");
            GenerarEnlaces(autorDTO, esAdmin.Succeeded);
        }

        private void GenerarEnlaces(AutorDTO autorDTO, bool esAdmin)
        {
            autorDTO.Enlaces.Add(new DatosHATEOASDTO(Enlace: linkGenerator.GetUriByRouteValues(httpContextAccessor.HttpContext!, "ObtenerAutorv1", new { id = autorDTO.Id })!, Descripcion: "self", Metodo: "GET"));
            if (esAdmin)
            {
                autorDTO.Enlaces.Add(new DatosHATEOASDTO(Enlace: linkGenerator.GetUriByRouteValues(httpContextAccessor.HttpContext!, "ActualizarAutorV1", new { id = autorDTO.Id })!, Descripcion: "autor-actualizar", Metodo: "PUT"));
                autorDTO.Enlaces.Add(new DatosHATEOASDTO(Enlace: linkGenerator.GetUriByRouteValues(httpContextAccessor.HttpContext!, "PatchAutorV1", new { id = autorDTO.Id })!, Descripcion: "autor-actualizar-parcial", Metodo: "PATCH"));
                autorDTO.Enlaces.Add(new DatosHATEOASDTO(Enlace: linkGenerator.GetUriByRouteValues(httpContextAccessor.HttpContext!, "BorrarAutorV1", new { id = autorDTO.Id })!, Descripcion: "autor-borrar", Metodo: "DELETE"));
            }


        }
    }
}
