using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Papeleria.Application.Interfaces;

namespace Papeleria.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacturasController : ControllerBase
    {
        private readonly IFacturaService _facturaService;

        public FacturasController(IFacturaService facturaService)
        {
            _facturaService = facturaService;
        }

        [HttpPost("confirmar")]
        [Authorize]
        public async Task<IActionResult> ConfirmarCompra()
        {
            var usuarioId = ObtenerUsuarioIdDesdeToken(); // Suponiendo que se extrae del token JWT
            var result = await _facturaService.ConfirmarCompraAsync(usuarioId);
            return StatusCode(result.StatusCode, result);
        }

        private Guid ObtenerUsuarioIdDesdeToken()
        {
            var claim = User.FindFirst("uid")?.Value;
            return Guid.TryParse(claim, out var id) ? id : Guid.Empty;
        }
    }
}
