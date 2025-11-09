using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Almuerzos.Infrastructure.DTOs
{
    public class ReservaDto
    {
        public int ReservaId { get; set; }
        public DateTime FechaReserva { get; set; } 
        public TimeSpan HoraSolicitada { get; set; } 
        public int NumeroPersonas { get; set; } 
        public string Estado { get; set; } 
        public DateTime FechaCreacion { get; set; }

        
        public ClienteDto Cliente { get; set; }

        
        public HorarioDto Horario { get; set; }
    }
}
