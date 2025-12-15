using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using Almuerzos.Core.Entities;
using Almuerzos.Core.Interfaces;
using Almuerzos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Dapper;

namespace Almuerzos.Infrastructure.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly AlmuerzosDbContext _context;
        private readonly IDbConnectionFactory _connectionFactory;

        public ClienteRepository(AlmuerzosDbContext context, IDbConnectionFactory connectionFactory)
        {
            _context = context;
            _connectionFactory = connectionFactory;
        }

        // --- Método GET con DAPPER (AJUSTADO) ---
        public async Task<Cliente> GetCliente(int id)
        {
            // Usamos AS para mapear cliente_id (DB) a ClienteId (Entity)
            var sql = "SELECT cliente_id AS ClienteId, nombre, apellido, email, telefono FROM Clientes WHERE cliente_id = @Id";

            using (var connection = _connectionFactory.CreateConnection())
            {
                var cliente = await connection.QueryFirstOrDefaultAsync<Cliente>(sql, new { Id = id });
                return cliente;
            }
        }

        public async Task<Cliente> GetClienteByEmail(string email)
        {
            // Mantenemos EF Core
            return await _context.Clientes.FirstOrDefaultAsync(c => c.email == email);
        }

        public async Task AddCliente(Cliente cliente)
        {
            await _context.Clientes.AddAsync(cliente);
        }

        public async Task<IEnumerable<Cliente>> GetClientes()
        {
            return await _context.Clientes.ToListAsync();
        }

        public async Task UpdateCliente(Cliente cliente)
        {
            _context.Clientes.Update(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return false;

            _context.Clientes.Remove(cliente);
            return true;
        }
    }
}