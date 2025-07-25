using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Papeleria.Application.DTOs;
using Papeleria.Application.Interfaces;
using Papeleria.Infrastructure.Services;

namespace Papeleria.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _service;

        public UsuariosController(IUsuarioService service)
        {
            _service = service;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] UsuarioRegistroDto dto)
        {
            var result = await _service.RegistrarAsync(dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UsuarioLoginDto dto)
        {
            var result = await _service.LoginAsync(dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("{id}/cambiar-password")]
        public async Task<IActionResult> CambiarPassword(Guid id, [FromBody] CambioPasswordDto dto)
        {
            var result = await _service.CambiarPasswordAsync(id, dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("enviar-recuperacion")]
        public async Task<IActionResult> EnviarToken([FromQuery] string email)
        {
            var result = await _service.EnviarRecuperacionPasswordAsync(email);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("recuperar-password")]
        public async Task<IActionResult> RecuperarPassword([FromBody] RecuperarPasswordDto dto)
        {
            var result = await _service.RecuperarPasswordAsync(dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("{id}/activar")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Activar(Guid id)
        {
            var result = await _service.ActivarUsuarioAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("{id}/desactivar")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Desactivar(Guid id)
        {
            var result = await _service.DesactivarUsuarioAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("{id}/cambiar-rol")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> CambiarRol(Guid id, [FromQuery] string nuevoRol)
        {
            var result = await _service.CambiarRolAsync(id, nuevoRol);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador,Gerente")]

        public async Task<IActionResult> Actualizar(Guid id, [FromBody] ActualizarUsuarioDto dto)
        {
            var result = await _service.ActualizarAsync(id, dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var result = await _service.ObtenerTodosAsync();
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("enviar-verificacion")]
        public async Task<IActionResult> EnviarVerificacion([FromBody] string email)
        {
            var result = await _service.EnviarVerificacionCuentaAsync(email);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("verificar")]
        public async Task<IActionResult> VerificarCuenta([FromQuery] string token)
        {
            var resultado = await _service.VerificarCuentaAsync(token);
            if (!resultado.Success)
            {
                return Content($"""
        <html>
        <head><title>Error de verificación</title></head>
        <body style="font-family:sans-serif; text-align:center; margin-top:100px; color:red;">
            <h1>❌ Error</h1>
            <p>{resultado.Message}</p>
        </body>
        </html>
        """, "text/html");
            }

            return Content($"""
    <html>
    <head><title>Cuenta verificada</title></head>
    <body style="font-family:sans-serif; text-align:center; margin-top:100px; color:green;">
        <h1>✅ Cuenta verificada</h1>
        <p>{resultado.Message}</p>
    </body>
    </html>
    """, "text/html");
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(Guid id)
        {
            var result = await _service.ObtenerPorIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
