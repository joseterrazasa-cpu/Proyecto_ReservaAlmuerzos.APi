using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Almuerzos.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Almuerzos.Infrastructure.Configurations
{
    public class ReservaConfiguration : IEntityTypeConfiguration<Reserva>
    {
        public void Configure(EntityTypeBuilder<Reserva> builder)
        {
            builder.ToTable("Reservas");

            builder.HasKey(r => r.reserva_id);

            builder.Property(r => r.fecha_reserva).IsRequired();
            builder.Property(r => r.hora_solicitada).IsRequired();
            builder.Property(r => r.numero_personas).IsRequired();
            builder.Property(r => r.estado).HasMaxLength(50).IsRequired();
            builder.Property(r => r.fecha_creacion).IsRequired();

            builder.HasOne(r => r.Cliente)
                .WithMany(c => c.Reservas)
                .HasForeignKey(r => r.cliente_id)
                .HasConstraintName("FK_Reservas_Clientes");

            builder.HasOne(r => r.Horario)
                .WithMany(h => h.Reservas)
                .HasForeignKey(r => r.horario_id)
                .HasConstraintName("FK_Reservas_Horarios");
        }
    }
}
