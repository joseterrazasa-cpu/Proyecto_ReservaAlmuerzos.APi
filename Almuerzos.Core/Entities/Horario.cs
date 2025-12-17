using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Almuerzos.Core.Entities
{
    public class Horario
    {
        public int horario_id { get; set; } 
        public int dia_semana { get; set; } 
        public TimeSpan hora_inicio { get; set; } 
        public TimeSpan hora_fin { get; set; } 
        public int capacidad_maxima { get; set; } 
        public ICollection<Reserva> Reservas { get; set; }

        [NotMapped]
        public object Descripcion { get; internal set; }
    }
}
