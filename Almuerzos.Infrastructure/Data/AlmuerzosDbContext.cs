using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Almuerzos.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection; 
using Almuerzos.Infrastructure.Configurations; 

namespace Almuerzos.Infrastructure.Data
{
    public class AlmuerzosDbContext : DbContext
    {
        public AlmuerzosDbContext(DbContextOptions<AlmuerzosDbContext> options)
            : base(options)
        {
        }

        
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Horario> Horarios { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<Plato> Platos { get; set; }
        public DbSet<DetallePedido> DetallePedidos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            
            base.OnModelCreating(modelBuilder);
        }
    }
}
