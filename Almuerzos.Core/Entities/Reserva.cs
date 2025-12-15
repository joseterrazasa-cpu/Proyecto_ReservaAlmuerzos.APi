using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Almuerzos.Core.Entities
{
    public class Reserva
    {
        public int reserva_id { get; set; } 
        public int cliente_id { get; set; } 
        public int horario_id { get; set; } 
        public DateTime fecha_reserva { get; set; } 
        public TimeSpan hora_solicitada { get; set; } 
        public int numero_personas { get; set; } 
        public string estado { get; set; } = "Pendiente"; 
        public DateTime fecha_creacion { get; set; } = DateTime.Now; 

        
        public Cliente Cliente { get; set; } 
        public Horario Horario { get; set; } 

        
        public Pago Pago { get; set; } 
        public ICollection<DetallePedido> DetallesPedido { get; set; } 
    }
}
