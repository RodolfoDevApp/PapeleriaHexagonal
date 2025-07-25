using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Papeleria.Application.DTOs;
using Papeleria.Application.Interfaces;
using Papeleria.Domain.Entities;
using Papeleria.Infrastructure.Persistence;
using Papeleria.WebApi.DTOs;

namespace Papeleria.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly IProductoService _service;

        public ProductosController(IProductoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<ProductoDto>>> GetAll([FromQuery] string? busqueda, [FromQuery] Guid? categoriaId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _service.ObtenerTodosAsync(busqueda, categoriaId, page, pageSize);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResultDto<ProductoDto>>> GetById(Guid id)
        {
            var result = await _service.ObtenerPorIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<ResultDto<string>>> Create([FromBody] CreateProductoDto dto)
        {
            var result = await _service.CrearAsync(dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<ResultDto<string>>> Update(Guid id, [FromBody] UpdateProductoDto dto)
        {
            var result = await _service.ActualizarAsync(id, dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<ResultDto<string>>> Delete(Guid id)
        {
            var result = await _service.EliminarAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
