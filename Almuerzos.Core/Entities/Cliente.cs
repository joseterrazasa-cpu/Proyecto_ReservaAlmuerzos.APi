using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Almuerzos.Core.Entities
{
    public class Cliente
    {
        public int cliente_id { get; set; } 
        public string nombre { get; set; } 
        public string apellido { get; set; } 
        public string email { get; set; } 
        public string telefono { get; set; } 
        public ICollection<Reserva> Reservas { get; set; }
    }
}
