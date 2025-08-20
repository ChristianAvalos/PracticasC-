using BivliotecaAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BivliotecaAPI.Controllers.V1
{
    [ApiController]
    [Route("api/v1")]
    [Authorize]
    public class RootController : ControllerBase
    {
        private readonly IAuthorizationService authorizationService;

        public RootController(IAuthorizationService authorizationService)
        {
            this.authorizationService = authorizationService;
        }
        [HttpGet(Name = "ObtenerRootV1")]
        [AllowAnonymous]
        public async Task<IEnumerable<DatosHATEOASDTO>> Get()
        {
            var datosHATEOAS = new List<DatosHATEOASDTO>();

            var esAdmin = await authorizationService.AuthorizeAsync(User, "esAdmin");

            //aaciones que cualquiera puede hacer
            datosHATEOAS.Add(new DatosHATEOASDTO(Enlace: Url.Link("ObtenerRootV1", new { })!,Descripcion: "self",Metodo: "GET"));
            datosHATEOAS.Add(new DatosHATEOASDTO(Enlace: Url.Link("ObtenerAutoresV1", new { })!, Descripcion: "autores-obtener", Metodo: "GET"));
            datosHATEOAS.Add(new DatosHATEOASDTO(Enlace: Url.Link("ObtenerLibrosV1", new { })!, Descripcion: "libros-obtener", Metodo: "GET"));
            datosHATEOAS.Add(new DatosHATEOASDTO(Enlace: Url.Link("RegistarUsuarioV1", new { })!, Descripcion: "usuarios-registrar", Metodo: "POST"));


            //acciones para usuarios autenticados
            if (User.Identity!.IsAuthenticated)
            {
                datosHATEOAS.Add(new DatosHATEOASDTO(Enlace: Url.Link("actualizarUsuario", new { })!, Descripcion: "actualizar-usuario", Metodo: "PUT"));
                datosHATEOAS.Add(new DatosHATEOASDTO(Enlace: Url.Link("renovar-token", new { })!, Descripcion: "RenovarTokenUsuarioV1", Metodo: "PUT"));
            }

            //acciones que solo un administrador puede hacer
            if (esAdmin.Succeeded)
            {
                datosHATEOAS.Add(new DatosHATEOASDTO(Enlace: Url.Link("CrearAutoresV1", new { })!, Descripcion: "autores-crear", Metodo: "POST"));
                datosHATEOAS.Add(new DatosHATEOASDTO(Enlace: Url.Link("CrearComentarioV1", new { libroId = 1 })!, Descripcion: "comentarios-crear", Metodo: "POST"));
                datosHATEOAS.Add(new DatosHATEOASDTO(Enlace: Url.Link("ObtenerUsuariosV1", new { })!, Descripcion: "usuarios-obtener", Metodo: "GET"));
                datosHATEOAS.Add(new DatosHATEOASDTO(Enlace: Url.Link("CrearLibroV1", new { })!, Descripcion: "libros-crear", Metodo: "POST"));
                datosHATEOAS.Add(new DatosHATEOASDTO(Enlace: Url.Link("ObtenerAutoresPorIdsv1", new { ids = "1,2,3" })!, Descripcion: "autores-coleccion-obtener-por-ids", Metodo: "GET"));
                datosHATEOAS.Add(new DatosHATEOASDTO(Enlace: Url.Link("ActualizarLibroV1", new { id = 1 })!, Descripcion: "libros-actualizar", Metodo: "PUT"));
                datosHATEOAS.Add(new DatosHATEOASDTO(Enlace: Url.Link("EliminarLibroV1", new { id = 1 })!, Descripcion: "libros-eliminar", Metodo: "DELETE"));
                datosHATEOAS.Add(new DatosHATEOASDTO(Enlace: Url.Link("ActualizarComentarioV1", new { id = Guid.NewGuid() })!, Descripcion: "comentarios-actualizar", Metodo: "PUT"));
                datosHATEOAS.Add(new DatosHATEOASDTO(Enlace: Url.Link("EliminarComentarioV1", new { id = Guid.NewGuid() })!, Descripcion: "comentarios-eliminar", Metodo: "DELETE"));
                datosHATEOAS.Add(new DatosHATEOASDTO(Enlace: Url.Link("ObtenerComentariosPorUsuarioV1", new { })!, Descripcion: "comentarios-obtener-por-usuario", Metodo: "GET"));
                datosHATEOAS.Add(new DatosHATEOASDTO(Enlace: Url.Link("CrearAutoresColeccionV1", new { })!, Descripcion: "autores-coleccion-crear", Metodo: "POST"));
            }

            return datosHATEOAS;
        }
         
    }
}
