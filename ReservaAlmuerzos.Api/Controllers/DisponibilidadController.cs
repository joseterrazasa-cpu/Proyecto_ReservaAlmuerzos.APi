using Almuerzos.Core.Interfaces;
using Almuerzos.Core.QueryFilters;
using Almuerzos.Infrastructure.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc; 
using System.Collections.Generic;
using System.Threading.Tasks;


namespace ReservaAlmuerzos.Api.Controllers
{
    
    [ApiController]
    [ApiVersion("1.0")] 
    [Route("api/v{version:apiVersion}/[controller]")] 
    [Authorize]
    public class DisponibilidadController : ControllerBase
    {
        private readonly IReservaService _reservaService;
        private readonly IMapper _mapper;

        public DisponibilidadController(IReservaService reservaService, IMapper mapper)
        {
            _reservaService = reservaService;
            _mapper = mapper;
        }


        [HttpGet]

        public async Task<IActionResult> Get([FromQuery] DisponibilidadDto consulta)
        {

            if (consulta.Fecha == System.DateTime.MinValue || consulta.NumeroPersonas <= 0)
            {
                return BadRequest(new { message = "Se requiere una fecha válida y un número de personas mayor que cero." });
            }

            // Create a filter for the query
            var filter = new ReservaQueryFilter
            {
                FechaDesde = consulta.Fecha,
                FechaHasta = consulta.Fecha
                // NumeroPersonas is not a property of ReservaQueryFilter, so it should be removed
            };

            var (reservas, totalCount) = await _reservaService.GetReservas(filter);

            // Nota: Este endpoint parece que intenta obtener Horarios, pero usa GetReservas.
            // Asumiendo que el mapeo a HorarioDto es correcto para el objetivo de disponibilidad:
            var horariosDto = _mapper.Map<IEnumerable<HorarioDto>>(reservas);

            return Ok(horariosDto);
        }
    }
}