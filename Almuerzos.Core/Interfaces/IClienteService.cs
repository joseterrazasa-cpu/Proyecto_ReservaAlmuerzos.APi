using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Almuerzos.Core.Entities;
using System.Collections.Generic;

namespace Almuerzos.Core.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio de lógica de negocio de Clientes.
    /// </summary>
    public interface IClienteService
    {
        Task<IEnumerable<Cliente>> GetClientes();
        Task<Cliente> GetCliente(int id);
        Task<bool> AddCliente(Cliente cliente);
        Task<bool> UpdateCliente(Cliente cliente);
        Task<bool> DeleteCliente(int id);
    }
}
