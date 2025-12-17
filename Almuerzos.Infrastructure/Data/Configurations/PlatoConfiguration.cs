using Almuerzos.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Almuerzos.Infrastructure.Configurations
{
    public class PlatoConfiguration : IEntityTypeConfiguration<Plato>
    {
        public void Configure(EntityTypeBuilder<Plato> builder)
        {
            builder.ToTable("Platos");

            // Primary key explícita (corrige el error)
            builder.HasKey(p => p.plato_id)
                   .HasName("PK_Plato");

            builder.Property(p => p.nombre)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(p => p.descripcion)
                .HasMaxLength(500);

            builder.Property(p => p.precio)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            // Si Plato tiene colecciones navegacionales, configúralas aquí (opcional).
        }
    }
}
