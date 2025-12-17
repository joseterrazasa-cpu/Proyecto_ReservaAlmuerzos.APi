using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Almuerzos.Core.Entities
{
    public class DetallePedido
    {
        public int detalle_id { get; set; } 
        public int reserva_id { get; set; } 
        public int plato_id { get; set; } 
        public int cantidad { get; set; } 
        public decimal precio_unitario { get; set; } 
        public Reserva Reserva { get; set; } 
        public Plato Plato { get; set; } 
    }
}
