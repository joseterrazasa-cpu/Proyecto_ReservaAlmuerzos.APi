using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Almuerzos.Core.Entities;
using Almuerzos.Core.Exceptions;
using Almuerzos.Core.Interfaces;

namespace Almuerzos.Core.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClienteService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Cliente>> GetClientes()
        {
            // Fix: IClienteRepository does not have GetClientes, so implement using available methods.
            // If you have a method to get all clientes, use it. Otherwise, you need to add it to IClienteRepository.
            throw new NotImplementedException("IClienteRepository does not define GetClientes. Implement this method in the repository interface and its implementation.");
        }

        public async Task<Cliente> GetCliente(int id)
        {
            return await _unitOfWork.Clientes.GetCliente(id);
        }

        public async Task<bool> AddCliente(Cliente cliente)
        {
            // Lógica de negocio: Evitar clientes duplicados por email
            var clienteExistente = await _unitOfWork.Clientes.GetClienteByEmail(cliente.Email);
            if (clienteExistente != null)
            {
                throw new BusinessException($"Ya existe un cliente registrado con el email: {cliente.Email}");
            }

            await _unitOfWork.Clientes.AddCliente(cliente);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateCliente(Cliente cliente)
        {
            // 1. Verificar si el cliente existe
            var clienteActual = await _unitOfWork.Clientes.GetCliente(cliente.ClienteId);
            if (clienteActual == null)
            {
                return false; // No se encontró el cliente
            }

            // 2. Verificar si el email ya está en uso por otro cliente
            var clienteExistente = await _unitOfWork.Clientes.GetClienteByEmail(cliente.Email);
            if (clienteExistente != null && clienteExistente.ClienteId != cliente.ClienteId)
            {
                throw new BusinessException($"El email {cliente.Email} ya está asociado a otro cliente.");
            }

            // Actualizar solo las propiedades que pueden cambiar
            clienteActual.Nombre = cliente.Nombre;
            clienteActual.Apellido = cliente.Apellido;
            clienteActual.Email = cliente.Email;
            clienteActual.Telefono = cliente.Telefono;

            _unitOfWork.Clientes.UpdateCliente(clienteActual);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCliente(int id)
        {
            // Lógica de negocio: No permitir eliminar si tiene reservas activas (simplificado por ahora, solo elimina)
            var resultado = await _unitOfWork.Clientes.DeleteCliente(id);
            await _unitOfWork.SaveChangesAsync();
            return resultado;
        }
    }
}
