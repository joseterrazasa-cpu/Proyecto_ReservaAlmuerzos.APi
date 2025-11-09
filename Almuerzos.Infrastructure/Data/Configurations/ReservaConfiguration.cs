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

            builder.HasKey(r => r.ReservaId); 

            builder.Property(r => r.Estado)
                .IsRequired() 
                .HasMaxLength(50)
                .HasDefaultValue("Pendiente"); 

             
            builder.ToTable(t => t.HasCheckConstraint("CHK_NumeroPersonas", "numero_personas > 0"));

            
            builder.HasOne<Cliente>()
                .WithMany()
                .HasForeignKey(r => r.ClienteId)
                .HasConstraintName("FK_Reserva_Cliente")
                .IsRequired();

            
            builder.HasOne<Horario>()
                .WithMany()
                .HasForeignKey(r => r.HorarioId)
                .HasConstraintName("FK_Reserva_Horario")
                .IsRequired();

            // Configuración de la hora solicitada
            builder.Property(r => r.HoraSolicitada)
                .HasColumnType("TIME");
        }
    }
}
