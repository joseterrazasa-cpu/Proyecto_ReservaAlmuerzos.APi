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
            
            throw new NotImplementedException("IClienteRepository does not define GetClientes. Implement this method in the repository interface and its implementation.");
        }

        public async Task<Cliente> GetCliente(int id)
        {
            return await _unitOfWork.Clientes.GetCliente(id);
        }

        public async Task<bool> AddCliente(Cliente cliente)
        {
            
            var clienteExistente = await _unitOfWork.Clientes.GetClienteByEmail(cliente.email);
            if (clienteExistente != null)
            {
                throw new BusinessException($"Ya existe un cliente registrado con el email: {cliente.email}");
            }

            await _unitOfWork.Clientes.AddCliente(cliente);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateCliente(Cliente cliente)
        {
            
            var clienteActual = await _unitOfWork.Clientes.GetCliente(cliente.cliente_id);
            if (clienteActual == null)
            {
                return false; 
            }

            
            var clienteExistente = await _unitOfWork.Clientes.GetClienteByEmail(cliente.email);
            if (clienteExistente != null && clienteExistente.cliente_id != cliente.cliente_id)
            {
                throw new BusinessException($"El email {cliente.email} ya está asociado a otro cliente.");
            }

            
            clienteActual.nombre = cliente.nombre;
            clienteActual.apellido = cliente.apellido;
            clienteActual.email = cliente.email;
            clienteActual.telefono = cliente.telefono;

            _unitOfWork.Clientes.UpdateCliente(clienteActual);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCliente(int id)
        {
            
            var resultado = await _unitOfWork.Clientes.DeleteCliente(id);
            await _unitOfWork.SaveChangesAsync();
            return resultado;
        }
    }
}
