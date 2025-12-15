using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Almuerzos.Core.Entities
{
    public class Pago
    {
        public int pago_id { get; set; } 
        public int reserva_id { get; set; } 
        public decimal monto { get; set; } 
        public string metodo_pago { get; set; } 
        public string estado_pago { get; set; } 
        public DateTime fecha_pago { get; set; } 

        
        public Reserva Reserva { get; set; } 
    }
}
