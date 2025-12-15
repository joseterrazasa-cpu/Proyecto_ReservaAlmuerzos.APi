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
    public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("Clientes"); 

            builder.HasKey(c => c.cliente_id); 

            builder.Property(c => c.nombre)
                .IsRequired() 
                .HasMaxLength(100);

            builder.Property(c => c.apellido)
                .HasMaxLength(100);

            builder.Property(c => c.email)
                .IsRequired() 
                .HasMaxLength(150);

            
            builder.HasIndex(c => c.email).IsUnique();

            builder.Property(c => c.telefono)
                .HasMaxLength(20);
        }
    }
}
