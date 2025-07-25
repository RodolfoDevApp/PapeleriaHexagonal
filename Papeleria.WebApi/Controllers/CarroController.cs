using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Papeleria.Application.DTOs;
using Papeleria.Application.Interfaces;

namespace Papeleria.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CarroController : ControllerBase
    {
        private readonly ICarroService _carroService;
        private readonly IPdfService _pdf;

        public CarroController(ICarroService carroService, IPdfService pdf)
        {
            _carroService = carroService;
            _pdf = pdf;
        }

        [HttpPost("agregar")]
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> Agregar([FromQuery] Guid usuarioId, [FromBody] AgregarItemCarritoDto dto)
        {
            var result = await _carroService.AgregarItemAsync(usuarioId, dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("eliminar")]
        public async Task<IActionResult> Eliminar([FromQuery] Guid usuarioId, [FromQuery] Guid productoId)
        {
            var result = await _carroService.EliminarItemAsync(usuarioId, productoId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> Obtener([FromQuery] Guid usuarioId)
        {
            var result = await _carroService.ObtenerCarritoAsync(usuarioId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("vaciar")]
        public async Task<IActionResult> Vaciar([FromQuery] Guid usuarioId)
        {
            var result = await _carroService.VaciarCarritoAsync(usuarioId);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost("{usuarioId}/confirmar-compra")]
        public async Task<IActionResult> ConfirmarCompra(Guid usuarioId)
        {
            var result = await _carroService.ConfirmarCompraAsync(usuarioId);

            if (!result.Success)
                return StatusCode(result.StatusCode, result);

            var pdf = await _pdf.GenerarFacturaPdfAsync(result.Data!.Id);
            return File(pdf, "application/pdf", $"Factura_{result.Data.Id}.pdf");
        }

    }
}
