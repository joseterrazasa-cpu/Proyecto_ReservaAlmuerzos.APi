using Almuerzos.Core.Entities;
using Almuerzos.Core.Interfaces;
using Almuerzos.Infrastructure.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc; 
using ReservaAlmuerzos.Api.Responses;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;


namespace ReservaAlmuerzos.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")] 
    [Route("api/v{version:apiVersion}/[controller]")] 
    [Authorize]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;
        private readonly IMapper _mapper;

        public ClientesController(IClienteService clienteService, IMapper mapper)
        {
            _clienteService = clienteService;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtiene un listado de todos los clientes.
        /// </summary>
        /// <returns>Una respuesta estandarizada conteniendo la lista de Clientes.</returns>
        /// <response code="200">Devuelve la lista de clientes.</response>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<ClienteDto>>))]
        public async Task<IActionResult> GetClientes()
        {
            var clientes = await _clienteService.GetClientes();
            var clientesDto = _mapper.Map<IEnumerable<ClienteDto>>(clientes);

            return Ok(new ApiResponse<IEnumerable<ClienteDto>>(clientesDto, "Lista de clientes recuperada exitosamente."));
        }

        /// <summary>
        /// Obtiene un cliente específico por su ID.
        /// </summary>
        /// <param name="id">ID del cliente.</param>
        /// <returns>El Cliente correspondiente dentro de una respuesta estandarizada.</returns>
        /// <response code="200">Devuelve el cliente encontrado.</response>
        /// <response code="404">Si no se encuentra el cliente.</response>
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<ClienteDto>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetCliente(int id)
        {
            var cliente = await _clienteService.GetCliente(id);
            if (cliente == null)
            {
                return NotFound(new ApiResponse<ClienteDto>($"No se encontró un cliente con ID: {id}"));
            }
            var clienteDto = _mapper.Map<ClienteDto>(cliente);

            return Ok(new ApiResponse<ClienteDto>(clienteDto));
        }

        /// <summary>
        /// Crea un nuevo cliente.
        /// </summary>
        /// <param name="clienteDto">Datos del nuevo cliente (CrearClienteDto).</param>
        /// <returns>Confirmación de creación.</returns>
        /// <response code="201">El cliente fue creado exitosamente.</response>
        /// <response code="400">Si los datos son inválidos o el email ya existe.</response>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(ApiResponse<bool>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CrearCliente([FromBody] CrearClienteDto clienteDto)
        {
            var cliente = _mapper.Map<Cliente>(clienteDto);

            var resultado = await _clienteService.AddCliente(cliente);

            if (resultado)
            {
                return StatusCode((int)HttpStatusCode.Created, new ApiResponse<bool>(true, "Cliente creado exitosamente."));
            }

            return BadRequest(new ApiResponse<bool>(false, "Fallo al crear el cliente."));
        }

        /// <summary>
        /// Actualiza un cliente existente.
        /// </summary>
        /// <param name="id">ID del cliente a actualizar.</param>
        /// <param name="clienteDto">Datos del cliente para actualizar (ModificarClienteDto).</param>
        /// <returns>Confirmación de actualización.</returns>
        /// <response code="200">El cliente fue actualizado.</response>
        /// <response code="404">Si el cliente no existe.</response>
        /// <response code="400">Si el email está duplicado.</response>
        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<bool>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateCliente(int id, [FromBody] ModificarClienteDto clienteDto)
        {
            var cliente = _mapper.Map<Cliente>(clienteDto);
            cliente.ClienteId = id; // Asignar el ID de la ruta

            var resultado = await _clienteService.UpdateCliente(cliente);

            if (resultado)
            {
                return Ok(new ApiResponse<bool>(true, "Cliente actualizado exitosamente."));
            }

            return NotFound(new ApiResponse<bool>(false, $"No se encontró el cliente con ID: {id} para actualizar."));
        }

        /// <summary>
        /// Elimina un cliente por su ID.
        /// </summary>
        /// <param name="id">ID del cliente a eliminar.</param>
        /// <returns>Confirmación de eliminación.</returns>
        /// <response code="200">El cliente fue eliminado.</response>
        /// <response code="404">Si el cliente no existe.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<bool>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var resultado = await _clienteService.DeleteCliente(id);

            if (resultado)
            {
                return Ok(new ApiResponse<bool>(true, "Cliente eliminado exitosamente."));
            }

            return NotFound(new ApiResponse<bool>(false, $"No se encontró el cliente con ID: {id} para eliminar."));
        }
    }
}