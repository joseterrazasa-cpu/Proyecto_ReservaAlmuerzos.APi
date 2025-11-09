using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Almuerzos.Core.Entities
{
    public class DetallePedido
    {
        public int DetalleId { get; set; } 
        public int ReservaId { get; set; } 
        public int PlatoId { get; set; } 
        public int Cantidad { get; set; } 
        public decimal PrecioUnitario { get; set; } 
        public Reserva Reserva { get; set; } 
        public Plato Plato { get; set; } 
    }
}
