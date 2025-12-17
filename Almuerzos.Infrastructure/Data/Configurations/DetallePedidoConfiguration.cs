using Almuerzos.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Almuerzos.Infrastructure.Configurations
{
    public class DetallePedidoConfiguration : IEntityTypeConfiguration<DetallePedido>
    {
        public void Configure(EntityTypeBuilder<DetallePedido> builder)
        {
            builder.ToTable("DetallePedidos");

            // Primary Key
            builder.HasKey(d => d.detalle_id);

            builder.Property(d => d.cantidad)
                .IsRequired();

            builder.Property(d => d.precio_unitario)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            // Foreign key -> Reserva (many DetallePedido por Reserva)
            builder.HasOne(d => d.Reserva)
                .WithMany(r => r.DetallesPedido)
                .HasForeignKey(d => d.reserva_id)
                .HasConstraintName("FK_DetallePedido_Reserva")
                .IsRequired();

            // Foreign key -> Plato
            builder.HasOne(d => d.Plato)
                .WithMany()
                .HasForeignKey(d => d.plato_id)
                .HasConstraintName("FK_DetallePedido_Plato")
                .IsRequired();
        }
    }
}
