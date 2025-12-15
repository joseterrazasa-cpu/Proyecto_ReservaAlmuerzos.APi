using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Almuerzos.Infrastructure.DTOs
{
    public class DisponibilidadDto
    {

        /// <summary>
        /// Fecha de la reserva
        /// </summary>
        [Required]
        public DateTime Fecha { get; set; }

        /// <summary>
        /// Número de personas
        /// </summary>
        [Required]
        public int NumeroPersonas { get; set; }
    }
}
