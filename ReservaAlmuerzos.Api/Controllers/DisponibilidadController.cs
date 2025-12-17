using Almuerzos.Core.Interfaces;
using Almuerzos.Core.QueryFilters;
using Almuerzos.Infrastructure.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReservaAlmuerzos.Api.Controllers
{
    /// <summary>
    /// Obtiene la disponibilidad de horarios según fecha y número de personas
    /// </summary>
    /// <remarks>
    /// Permite consultar qué horarios están disponibles para una fecha específica
    /// indicando la cantidad de personas.
    /// </remarks>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    //[Authorize]
    public class DisponibilidadController : ControllerBase
    {
        private readonly IReservaService _reservaService;
        private readonly IMapper _mapper;

        public DisponibilidadController(
            IReservaService reservaService,
            IMapper mapper)
        {
            _reservaService = reservaService;
            _mapper = mapper;
        }

        /// <summary>
        /// Consulta los horarios disponibles según fecha y número de personas
        /// </summary>
        /// <remarks>
        /// Ejemplo de solicitud:
        ///
        ///     GET /api/v1/Disponibilidad?Fecha=2025-06-15&NumeroPersonas=4
        ///
        /// </remarks>
        /// <param name="consulta">Fecha de la reserva y cantidad de personas</param>
        /// <returns>Lista de horarios disponibles</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<HorarioDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Get([FromQuery] DisponibilidadDto consulta)
        {
            if (consulta.Fecha == DateTime.MinValue || consulta.NumeroPersonas <= 0)
            {
                return BadRequest(new
                {
                    message = "Se requiere una fecha válida y un número de personas mayor que cero."
                });
            }

            var filter = new ReservaQueryFilter
            {
                FechaDesde = consulta.Fecha,
                FechaHasta = consulta.Fecha
            };

            var (reservas, totalCount) = await _reservaService.GetReservas(filter);

            var horariosDto = _mapper.Map<IEnumerable<HorarioDto>>(reservas);

            return Ok(horariosDto);
        }
    }
}
