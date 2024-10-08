using Microsoft.AspNetCore.Mvc;
using ServicioDLL.Data.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace proyecto_practira04_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiciosController : ControllerBase
    {

        private IServicioRepository _repository;

        public ServiciosController(IServicioRepository repository)
        {
            _repository = repository;
        }



        [HttpGet("con filtros")]
        public IActionResult GetByNameCost(string? nombre = null, int? costo = null)
        {
            var servicios = _repository.GetByNameCost(nombre, costo);

            if (servicios == null || !servicios.Any())
            {
                return NotFound("No se encontraron servicios que coincidan con los criterios.");
            }

            return Ok(servicios);
        }
    }
}
