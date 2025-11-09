using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Almuerzos.Core.Entities
{
    public class Cliente
    {
        public int ClienteId { get; set; } 
        public string Nombre { get; set; } 
        public string Apellido { get; set; } 
        public string Email { get; set; } 
        public string Telefono { get; set; } 
        public ICollection<Reserva> Reservas { get; set; }
    }
}
