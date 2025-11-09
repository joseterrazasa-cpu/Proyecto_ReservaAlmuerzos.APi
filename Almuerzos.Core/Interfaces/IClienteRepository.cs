using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Almuerzos.Core.Entities;

namespace Almuerzos.Core.Interfaces
{
    public interface IClienteRepository
    {
        Task<Cliente> GetCliente(int id);
        Task AddCliente(Cliente cliente);
        Task<Cliente> GetClienteByEmail(string email);
        Task UpdateCliente(Cliente cliente);
        Task<bool> DeleteCliente(int id);
    }
}
