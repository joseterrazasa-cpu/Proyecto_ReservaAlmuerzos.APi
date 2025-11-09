using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Almuerzos.Core.Entities
{
    public class Horario
    {
        public int HorarioId { get; set; } 
        public int DiaSemana { get; set; } 
        public TimeSpan HoraInicio { get; set; } 
        public TimeSpan HoraFin { get; set; } 
        public int CapacidadMaxima { get; set; } 
        public ICollection<Reserva> Reservas { get; set; }
        public object Descripcion { get; internal set; }
    }
}
