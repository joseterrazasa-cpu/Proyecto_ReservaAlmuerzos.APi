using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Almuerzos.Core.Entities
{
    public class Plato
    {
        public int plato_id { get; set; } 
        public string nombre { get; set; } 
        public string descripcion { get; set; } 
        public decimal precio { get; set; } 
        public string categoria { get; set; } 
        public bool activo { get; set; } = true; 

        
        public ICollection<DetallePedido> DetallesPedido { get; set; }
    }
}
