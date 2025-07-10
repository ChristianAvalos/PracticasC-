using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace BivliotecaAPI.Controllers
{
    [Route("api/seguridad")]
    [ApiController]
    public class SeguridadController : ControllerBase
    {

        private readonly IDataProtector protector;
        private readonly ITimeLimitedDataProtector protectorLimitadoPorTimepo;

        public SeguridadController(IDataProtectionProvider protectionProvider)
        {
            protector = protectionProvider.CreateProtector("SeguridadController");
            protectorLimitadoPorTimepo = protector.ToTimeLimitedDataProtector();
        }

        [HttpGet("encriptar")]
        public ActionResult<string> Encriptar(string texto)
        {
            if (string.IsNullOrEmpty(texto))
            {
                return BadRequest("El texto no puede ser nulo o vacío.");
            }
            var textoEncriptado = protector.Protect(texto);
            return Ok(new { textoEncriptado });
        }
        [HttpGet("encriptar-limitado-por-tiempo")]
        public ActionResult<string> EncriptarlimitadoPorTiempo(string texto)
        {
            if (string.IsNullOrEmpty(texto))
            {
                return BadRequest("El texto no puede ser nulo o vacío.");
            }
            var textoEncriptado = protectorLimitadoPorTimepo.Protect(texto,lifetime: TimeSpan.FromSeconds(30));
            return Ok(new { textoEncriptado });
        }
        [HttpGet("desencriptar-limitado-por-tiempo")]
        public ActionResult<string> DesencriptarlimitadoPorTiempo(string textoEncriptado)
        {
            if (string.IsNullOrEmpty(textoEncriptado))
            {
                return BadRequest("El texto encriptado no puede ser nulo o vacío.");
            }
            try
            {
                var textoDesencriptado = protectorLimitadoPorTimepo.Unprotect(textoEncriptado);
                return Ok(new { textoDesencriptado });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al desencriptar el texto: {ex.Message}");
            }
        }
        [HttpGet("desencriptar")]
        public ActionResult<string> Desencriptar(string textoEncriptado)
        {
            if (string.IsNullOrEmpty(textoEncriptado))
            {
                return BadRequest("El texto encriptado no puede ser nulo o vacío.");
            }
            try
            {
                var textoDesencriptado = protector.Unprotect(textoEncriptado);
                return Ok(new { textoDesencriptado });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al desencriptar el texto: {ex.Message}");
            }
        }
    }
}
