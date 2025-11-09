using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Almuerzos.Core.Interfaces;
using Almuerzos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage; // Necesario para IDbContextTransaction
using System.Data;

namespace Almuerzos.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AlmuerzosDbContext _context;
        private readonly IDbConnectionFactory _connectionFactory;
        private IDbContextTransaction? _currentTransaction;

        // Implementación de las propiedades requeridas por la interfaz
        public IClienteRepository ClienteRepository { get; }
        public IHorarioRepository HorarioRepository { get; }
        public IReservaRepository ReservaRepository { get; }

        // Implementación explícita de las propiedades de la interfaz
        public IClienteRepository Clientes => ClienteRepository;
        public IHorarioRepository Horarios => HorarioRepository;
        public IReservaRepository Reservas => ReservaRepository;

        public UnitOfWork(AlmuerzosDbContext context, IDbConnectionFactory connectionFactory)
        {
            _context = context;
            _connectionFactory = connectionFactory;

            ClienteRepository = new ClienteRepository(_context, _connectionFactory);
            HorarioRepository = new HorarioRepository(_context, _connectionFactory);
            ReservaRepository = new ReservaRepository(_context, _connectionFactory);
        }

        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
            _currentTransaction?.Dispose();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        // Implementación de transacciones
        public async Task BeginTransaccionAsync()
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.DisposeAsync();
            }
            _currentTransaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.CommitAsync();
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }

        public async Task RollbackAsync()
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.RollbackAsync();
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }

        public IDbConnection? GetDbConnection()
        {
            return _connectionFactory?.CreateConnection();
        }

        public IDbTransaction? GetDbTransaction()
        {
            return _currentTransaction?.GetDbTransaction();
        }
    }
}