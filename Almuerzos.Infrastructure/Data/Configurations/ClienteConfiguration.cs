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

            builder.HasKey(c => c.ClienteId); 

            builder.Property(c => c.Nombre)
                .IsRequired() 
                .HasMaxLength(100);

            builder.Property(c => c.Apellido)
                .HasMaxLength(100);

            builder.Property(c => c.Email)
                .IsRequired() 
                .HasMaxLength(150);

            
            builder.HasIndex(c => c.Email).IsUnique();

            builder.Property(c => c.Telefono)
                .HasMaxLength(20);
        }
    }
}
