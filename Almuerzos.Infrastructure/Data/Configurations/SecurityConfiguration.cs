using Almuerzos.Core.Entities;
using Almuerzos.Core.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Almuerzos.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework Core para la entidad Security.
    /// </summary>
    public class SecurityConfiguration : IEntityTypeConfiguration<Security>
    {
        public void Configure(EntityTypeBuilder<Security> builder)
        {
            
            builder.ToTable("Security");

            
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            
            builder.Property(e => e.Login)
                .HasMaxLength(50)
                .IsRequired()
                .IsUnicode(false);

            
            builder.Property(e => e.Password)
                .HasMaxLength(200)
                .IsRequired()
                .IsUnicode(false);

            
            builder.Property(e => e.Name)
                .HasMaxLength(100)
                .IsRequired()
                .IsUnicode(false);

            builder.Property(e => e.Role)
                .HasMaxLength(15)
                .IsRequired()
                .IsUnicode(false)
                .HasConversion(
                    
                    x => x.ToString(),
                    
                    x => (RoleType)Enum.Parse(typeof(RoleType), x)
                );
        }
    }
}
