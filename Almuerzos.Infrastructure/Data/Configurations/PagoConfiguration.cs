using Almuerzos.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Almuerzos.Infrastructure.Configurations
{
    public class PagoConfiguration : IEntityTypeConfiguration<Pago>
    {
        public void Configure(EntityTypeBuilder<Pago> builder)
        {
            builder.ToTable("Pagos");

            builder.HasKey(p => p.pago_id);

            builder.Property(p => p.monto)
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.metodo_pago)
                .HasMaxLength(100);

            builder.Property(p => p.estado_pago)
                .HasMaxLength(50);

            builder.Property(p => p.fecha_pago);

            builder.Property(p => p.reserva_id)
                .IsRequired();

            // Indicar explícitamente que Pago es la entidad dependiente
            builder.HasOne(p => p.Reserva)
                .WithOne(r => r.Pago)
                .HasForeignKey<Pago>(p => p.reserva_id)
                .HasConstraintName("FK_Pago_Reserva");
        }
    }
}
