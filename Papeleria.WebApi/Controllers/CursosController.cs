using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Papeleria.Application.DTOs;
using Papeleria.Application.Interfaces;
using Papeleria.Domain.Entities;
using Papeleria.Infrastructure.Persistence;
using Papeleria.WebApi.DTOs;

namespace Papeleria.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CursosController : ControllerBase
    {
        private readonly ICursoService _cursoService;

        public CursosController(ICursoService cursoService)
        {
            _cursoService = cursoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? busqueda, [FromQuery] Guid? categoriaId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _cursoService.ObtenerTodosAsync(busqueda, categoriaId, page, pageSize);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _cursoService.ObtenerPorIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create([FromBody] CreateCursoDto dto)
        {
            var result = await _cursoService.CrearAsync(dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCursoDto dto)
        {
            var result = await _cursoService.ActualizarAsync(id, dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _cursoService.EliminarAsync(id);
            return StatusCode(result.StatusCode, result);
        }

    }
}
