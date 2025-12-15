using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data; 

namespace Almuerzos.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        
        IClienteRepository Clientes { get; }
        IHorarioRepository Horarios { get; }
        IReservaRepository Reservas { get; }
        ISecurityRepository SecurityRepository { get; }

        
        void SaveChanges();
        Task SaveChangesAsync(); 

        
        Task BeginTransaccionAsync();
        Task CommitAsync();
        Task RollbackAsync();

        
        IDbConnection? GetDbConnection();
        IDbTransaction? GetDbTransaction();
    }
}