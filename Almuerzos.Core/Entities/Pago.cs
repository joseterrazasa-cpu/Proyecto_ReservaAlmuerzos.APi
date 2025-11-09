using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Almuerzos.Core.Entities
{
    public class Pago
    {
        public int PagoId { get; set; } 
        public int ReservaId { get; set; } 
        public decimal Monto { get; set; } 
        public string MetodoPago { get; set; } 
        public string EstadoPago { get; set; } 
        public DateTime FechaPago { get; set; } 

        
        public Reserva Reserva { get; set; } 
    }
}
