using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Almuerzos.Infrastructure.DTOs
{
    public class HorarioDto
    {
        public int HorarioId { get; set; }
        public int DiaSemana { get; set; } 
        public TimeSpan HoraInicio { get; set; } 
        public TimeSpan HoraFin { get; set; } 
        public int CapacidadMaxima { get; set; } 
    }
}
