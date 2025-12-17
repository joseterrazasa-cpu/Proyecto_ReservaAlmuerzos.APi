using Almuerzos.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Almuerzos.Infrastructure.Configurations
{
    public class HorarioConfiguration : IEntityTypeConfiguration<Horario>
    {
        public void Configure(EntityTypeBuilder<Horario> builder)
        {
            builder.ToTable("Horarios");

            // Primary key explícita
            builder.HasKey(h => h.horario_id)
                   .HasName("PK_Horario");

            builder.Property(h => h.dia_semana)
                .IsRequired();

            builder.Property(h => h.hora_inicio)
                .HasColumnType("TIME")
                .IsRequired();

            builder.Property(h => h.hora_fin)
                .HasColumnType("TIME")
                .IsRequired();

            builder.Property(h => h.capacidad_maxima)
                .IsRequired();

            // Descripcion está marcada con [NotMapped], no es necesario configurarla aquí.
        }
    }
}
