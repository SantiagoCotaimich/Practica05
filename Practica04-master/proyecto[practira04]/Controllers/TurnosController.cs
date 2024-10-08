using Microsoft.AspNetCore.Mvc;
using ServicioDLL.Data.Repositories;
using TuNamespace.Models;
using TurnosDLL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TuNamespace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TurnoController : ControllerBase
    {
        private readonly ITurnoRepository _turnoRepository;

        public TurnoController(ITurnoRepository turnoRepository)
        {
            _turnoRepository = turnoRepository;
        }

        [HttpPost]
        public IActionResult Create([FromBody] TurnoRequest request)
        {
            if (request?.Turno == null || request.Servicios == null || !request.Servicios.Any())
            {
                return BadRequest("Datos de turno o servicios no válidos.");
            }

            try
            {
                var result = _turnoRepository.Create(request.Turno, request.Servicios);
                if (result)
                {
                    return Ok("Turno creado exitosamente.");
                }
                return BadRequest("No se pudo crear el turno.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpGet]
        public IActionResult GetByTimeCliente(DateTime? fecha, string? cliente)
        {
            try
            {
                var turnos = _turnoRepository.GetByTimeCliente(fecha, cliente);
                if (turnos == null || !turnos.Any())
                {
                    return NotFound("No se encontraron turnos para la búsqueda especificada.");
                }
                return Ok(turnos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] TurnoRequest request)
        {
            if (request?.Turno == null || request.Servicios == null || !request.Servicios.Any())
            {
                return BadRequest("Datos de turno o servicios no válidos.");
            }

            try
            {
                var result = _turnoRepository.Update(id, request.Turno, request.Servicios);
                if (result)
                {
                    return Ok("Turno actualizado exitosamente.");
                }
                return NotFound("Turno no encontrado o no se pudo actualizar.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id, [FromBody] string motivoCancelacion)
        {
            if (string.IsNullOrEmpty(motivoCancelacion))
            {
                return BadRequest("El motivo de cancelación es requerido.");
            }

            try
            {
                var result = _turnoRepository.Delete(id, motivoCancelacion);
                if (result)
                {
                    return Ok("Turno cancelado exitosamente.");
                }
                return NotFound("Turno no encontrado.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }
    }
}
