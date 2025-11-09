using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Almuerzos.Core.Entities
{
    public class Reserva
    {
        public int ReservaId { get; set; } 
        public int ClienteId { get; set; } 
        public int HorarioId { get; set; } 
        public DateTime FechaReserva { get; set; } 
        public TimeSpan HoraSolicitada { get; set; } 
        public int NumeroPersonas { get; set; } 
        public string Estado { get; set; } = "Pendiente"; 
        public DateTime FechaCreacion { get; set; } = DateTime.Now; 

        
        public Cliente Cliente { get; set; } 
        public Horario Horario { get; set; } 

        
        public Pago Pago { get; set; } 
        public ICollection<DetallePedido> DetallesPedido { get; set; } 
    }
}
