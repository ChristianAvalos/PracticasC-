using Microsoft.AspNetCore.Mvc;

namespace BivliotecaAPI.Controllers
{
    [ApiController]
    [Route("api/configuraciones")]
    public class ConfiguracionesController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IConfigurationSection seccion_01;
        private IConfigurationSection seccion_02;

        public ConfiguracionesController(IConfiguration configuration)
        {
            this.configuration = configuration;
            seccion_01 = configuration.GetSection("seccion_1");
            seccion_02 = configuration.GetSection("seccion_2");
        }
        [HttpGet("obtenertodos")]
        public ActionResult  GetObtenerTodos()
        {
            var hijos = configuration.GetChildren().Select(x => $"{x.Key}:{x.Value}");
            return Ok(new {hijos});
        }


        [HttpGet("seccion_01")]
        public ActionResult<string> GetSeccion01()
        {
            var nombre = seccion_01.GetValue<string>("nombre");
            var edad = seccion_01.GetValue<string>("edad");

            return Ok(new {nombre,edad});
        }

        [HttpGet("seccion_02")]
        public ActionResult<string> GetSeccion02()
        {
            var nombre = seccion_02.GetValue<string>("nombre");
            var edad = seccion_02.GetValue<string>("edad");

            return Ok(new { nombre, edad });
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            var valor = configuration["apellido"];
            if (string.IsNullOrEmpty(valor))
            {
                return NotFound("No se encontró el valor de configuración.");
            }
            return Ok(valor);
        }
        [HttpGet("secciones")]
        public ActionResult<string> GetSecciones()
        {
            var opcion1 = configuration["ConnectionStrings:DefaultConnection"];
            
            return Ok(opcion1);
        }

    }

}
