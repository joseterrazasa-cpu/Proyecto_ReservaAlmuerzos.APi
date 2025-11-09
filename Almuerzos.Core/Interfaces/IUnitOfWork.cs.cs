using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data; // Necesario para IDbConnection y IDbTransactio

namespace Almuerzos.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        // Repositorios de tu aplicación
        IClienteRepository Clientes { get; }
        IHorarioRepository Horarios { get; }
        IReservaRepository Reservas { get; }

        // Métodos de Guardado y Transacción (copiados del magíster)
        void SaveChanges();
        Task SaveChangesAsync(); // Lo usaremos para el Commit implícito de EF Core

        // Métodos para Transacciones Explícitas (necesarios para el control total)
        Task BeginTransaccionAsync();
        Task CommitAsync();
        Task RollbackAsync();

        // Miembros de Dapper (para las consultas de solo lectura de alto rendimiento)
        IDbConnection? GetDbConnection();
        IDbTransaction? GetDbTransaction();
    }
}