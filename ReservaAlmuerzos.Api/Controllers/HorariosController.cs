using Almuerzos.Core.Entities;
using Almuerzos.Core.Interfaces;
using Almuerzos.Infrastructure.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Mvc; 
using ReservaAlmuerzos.Api.Responses;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ReservaAlmuerzos.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")] 
    [Route("api/v{version:apiVersion}/[controller]")] 
    [Authorize]
    public class HorariosController : ControllerBase
    {
        private readonly IHorarioService _horarioService;
        private readonly IMapper _mapper;

        public HorariosController(IHorarioService horarioService, IMapper mapper)
        {
            _horarioService = horarioService;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtiene un listado de todos los horarios (turnos) disponibles.
        /// </summary>
        /// <returns>Una respuesta estandarizada conteniendo la lista de Horarios.</returns>
        /// <response code="200">Devuelve la lista de horarios.</response>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<HorarioDto>>))]
        public async Task<IActionResult> GetHorarios()
        {
            var horarios = await _horarioService.GetHorarios();
            var horariosDto = _mapper.Map<IEnumerable<HorarioDto>>(horarios);

            return Ok(new ApiResponse<IEnumerable<HorarioDto>>(horariosDto, "Lista de horarios recuperada exitosamente."));
        }

        /// <summary>
        /// Obtiene un horario específico por su ID.
        /// </summary>
        /// <param name="id">ID del horario.</param>
        /// <returns>El Horario correspondiente dentro de una respuesta estandarizada.</returns>
        /// <response code="200">Devuelve el horario encontrado.</response>
        /// <response code="404">Si no se encuentra el horario.</response>
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<HorarioDto>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetHorario(int id)
        {
            var horario = await _horarioService.GetHorario(id);
            if (horario == null)
            {
                return NotFound(new ApiResponse<HorarioDto>($"No se encontró un horario con ID: {id}"));
            }
            var horarioDto = _mapper.Map<HorarioDto>(horario);

            return Ok(new ApiResponse<HorarioDto>(horarioDto));
        }

        /// <summary>
        /// Crea un nuevo horario de turno.
        /// </summary>
        /// <param name="horarioDto">Datos del nuevo horario (CrearHorarioDto).</param>
        /// <returns>Confirmación de creación.</returns>
        /// <response code="201">El horario fue creado exitosamente.</response>
        /// <response code="400">Si los datos son inválidos (ej. HoraInicio >= HoraFin).</response>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(ApiResponse<bool>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CrearHorario([FromBody] CrearHorarioDto horarioDto)
        {
            var horario = _mapper.Map<Horario>(horarioDto);

            var resultado = await _horarioService.AddHorario(horario);

            if (resultado)
            {
                return StatusCode((int)HttpStatusCode.Created, new ApiResponse<bool>(true, "Horario creado exitosamente."));
            }

            return BadRequest(new ApiResponse<bool>(false, "Fallo al crear el horario."));
        }

        /// <summary>
        /// Actualiza un horario existente.
        /// </summary>
        /// <param name="id">ID del horario a actualizar.</param>
        /// <param name="horarioDto">Datos del horario para actualizar (ModificarHorarioDto).</param>
        /// <returns>Confirmación de actualización.</returns>
        /// <response code="200">El horario fue actualizado.</response>
        /// <response code="404">Si el horario no existe.</response>
        /// <response code="400">Si las horas son inválidas.</response>
        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<bool>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateHorario(int id, [FromBody] ModificarHorarioDto horarioDto)
        {
            var horario = _mapper.Map<Horario>(horarioDto);
            horario.horario_id = id; // Asignar el ID de la ruta

            var resultado = await _horarioService.UpdateHorario(horario);

            if (resultado)
            {
                return Ok(new ApiResponse<bool>(true, "Horario actualizado exitosamente."));
            }

            return NotFound(new ApiResponse<bool>(false, $"No se encontró el horario con ID: {id} para actualizar."));
        }

        /// <summary>
        /// Elimina un horario por su ID.
        /// </summary>
        /// <param name="id">ID del horario a eliminar.</param>
        /// <returns>Confirmación de eliminación.</returns>
        /// <response code="200">El horario fue eliminado.</response>
        /// <response code="404">Si el horario no existe.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<bool>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteHorario(int id)
        {
            var resultado = await _horarioService.DeleteHorario(id);

            if (resultado)
            {
                return Ok(new ApiResponse<bool>(true, "Horario eliminado exitosamente."));
            }

            return NotFound(new ApiResponse<bool>(false, $"No se encontró el horario con ID: {id} para eliminar."));
        }
    }
}