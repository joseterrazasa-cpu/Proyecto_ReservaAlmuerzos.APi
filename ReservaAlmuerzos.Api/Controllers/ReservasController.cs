using Almuerzos.Core.Entities;
using Almuerzos.Core.Interfaces;
using Almuerzos.Core.QueryFilters;
using Almuerzos.Infrastructure.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc; 
using ReservaAlmuerzos.Api.Responses;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;



namespace ReservaAlmuerzos.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")] 
    [Route("api/v{version:apiVersion}/[controller]")] 
    //[Authorize]
    public class ReservasController : ControllerBase
    {
        private readonly IReservaService _reservaService;
        private readonly IMapper _mapper;

        public ReservasController(IReservaService reservaService, IMapper mapper)
        {
            _reservaService = reservaService;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtiene un listado de reservas con paginación y filtrado opcional. (Requisito 7 y 8)
        /// La respuesta está estandarizada incluyendo 'Messages', 'Data' y 'Pagination'.
        /// </summary>
        /// <param name="filters">Filtros de paginación, tamaño de página y filtros de consulta por fecha.</param>
        /// <returns>Una respuesta estandarizada conteniendo la lista de Reservas y la metadata de paginación.</returns>
        /// <response code="200">Devuelve la lista de reservas paginadas.</response>
        /// <response code="400">Si los parámetros de la solicitud son inválidos.</response>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<ReservaDto>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetReservas([FromQuery] ReservaQueryFilter filters)
        {
            
            var (reservas, totalCount) = await _reservaService.GetReservas(filters);

            
            var reservasDto = _mapper.Map<IEnumerable<ReservaDto>>(reservas);

            
            var basePath = $"{Request.Scheme}://{Request.Host}{Request.Path.Value}";
            var totalPages = (int)Math.Ceiling((double)totalCount / filters.PageSize);
            var hasPreviousPage = filters.PageNumber > 1;
            var hasNextPage = filters.PageNumber < totalPages;

            var paginationMetadata = new PaginationMetadata(
                filters.PageNumber,
                filters.PageSize,
                totalCount
            )
            {
                TotalPages = totalPages,
                NextPageUrl = hasNextPage
                    ? $"{Request.Scheme}://{Request.Host}{Request.Path.Value}?PageNumber={filters.PageNumber + 1}&PageSize={filters.PageSize}"
                    : null,
                PreviousPageUrl = hasPreviousPage
                    ? $"{Request.Scheme}://{Request.Host}{Request.Path.Value}?PageNumber={filters.PageNumber - 1}&PageSize={filters.PageSize}"
                    : null
            };

            
            var response = new ApiResponse<IEnumerable<ReservaDto>>(reservasDto, paginationMetadata);

            
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            return Ok(response);
        }

        /// <summary>
        /// Obtiene una reserva específica por su ID. (Requisito 7 y 8)
        /// </summary>
        /// <param name="id">ID de la reserva.</param>
        /// <returns>La Reserva correspondiente dentro de una respuesta estandarizada.</returns>
        /// <response code="200">Devuelve la reserva encontrada.</response>
        /// <response code="404">Si no se encuentra la reserva.</response>
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<ReservaDto>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetReserva(int id)
        {
            var reserva = await _reservaService.GetReserva(id);
            if (reserva == null)
            {
                // Devolver NotFound estandarizado con ApiResponse
                return NotFound(new ApiResponse<ReservaDto>($"No se encontró una reserva con ID: {id}"));
            }
            var reservaDto = _mapper.Map<ReservaDto>(reserva);

            // Estandarizar la respuesta para GET por ID
            return Ok(new ApiResponse<ReservaDto>(reservaDto));
        }

        /// <summary>
        /// Crea una nueva reserva en el sistema. (Requisito 7 y 8)
        /// </summary>
        /// <param name="reservaDto">Datos de la nueva reserva (CrearReservaDto).</param>
        /// <returns>Confirmación de creación.</returns>
        /// <response code="201">La reserva fue creada exitosamente.</response>
        /// <response code="400">Si los datos son inválidos o falla la lógica de negocio.</response>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(ApiResponse<bool>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CrearReserva([FromBody] CrearReservaDto reservaDto)
        {
            var reserva = _mapper.Map<Reserva>(reservaDto);

            var resultado = await _reservaService.CrearReserva(reserva);

            if (resultado != null)
            {
                // Retornar código 201 Created con ApiResponse estandarizado
                return StatusCode((int)HttpStatusCode.Created, new ApiResponse<bool>(true, "Reserva creada exitosamente."));
            }

            return BadRequest(new ApiResponse<bool>(false, "Fallo al crear la reserva. Revise las validaciones."));
        }

        /// <summary>
        /// Actualiza una reserva existente. (Requisito 7 y 8)
        /// </summary>
        /// <param name="id">ID de la reserva a actualizar.</param>
        /// <param name="reservaDto">Datos de la reserva para actualizar (ModificarReservaDto).</param>
        /// <returns>Confirmación de actualización.</returns>
        /// <response code="200">La reserva fue actualizada.</response>
        /// <response code="404">Si la reserva no existe.</response>
        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<bool>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateReserva(int id, [FromBody] ReservaDto reservaDto)
        {
            var reserva = _mapper.Map<Reserva>(reservaDto);
            reserva.reserva_id = id; // Asignar el ID de la ruta

            var resultado = await _reservaService.UpdateReserva(reserva);

            if (resultado)
            {
                return Ok(new ApiResponse<bool>(true, "Reserva actualizada exitosamente."));
            }

            return NotFound(new ApiResponse<bool>(false, $"No se encontró la reserva con ID: {id} para actualizar."));
        }

        /// <summary>
        /// Cancela una reserva existente por su ID. (Requisito 7 y 8)
        /// </summary>
        /// <param name="id">ID de la reserva a cancelar.</param>
        /// <returns>Confirmación de cancelación.</returns>
        /// <response code="200">La reserva fue cancelada.</response>
        /// <response code="404">Si la reserva no existe.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<bool>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> CancelarReserva(int id)
        {
            var resultado = await _reservaService.CancelarReserva(id);

            if (resultado)
            {
                // Estandarizar la respuesta para DELETE
                return Ok(new ApiResponse<bool>(true, "Reserva cancelada exitosamente."));
            }

            // Retornar NotFound estandarizado with ApiResponse
            return NotFound(new ApiResponse<bool>(false, $"No se encontró la reserva con ID: {id} para cancelar."));
        }
    }
}