using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Almuerzos.Core.Entities;
using Almuerzos.Core.Interfaces;
using Almuerzos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Almuerzos.Infrastructure.Repositories
{
    /// <summary>
    /// Repositorio específico para la entidad Security.
    /// </summary>
    public class SecurityRepository : ISecurityRepository
    {
        private readonly AlmuerzosDbContext _context;
        private readonly DbSet<Security> _entities;

        public SecurityRepository(AlmuerzosDbContext context)
        {
            _context = context;
            _entities = _context.Set<Security>();
        }

        /// <summary>
        /// Busca un usuario por su Login (nombre de usuario).
        /// </summary>
        public async Task<Security> GetByLogin(string login)
        {

            return await _entities.FirstOrDefaultAsync(x => x.Login == login);
        }

        public async Task Add(Security entity)
        {
            await _entities.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
    }
}