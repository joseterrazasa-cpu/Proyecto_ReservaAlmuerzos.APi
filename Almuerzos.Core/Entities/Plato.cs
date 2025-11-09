using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Almuerzos.Core.Entities
{
    public class Plato
    {
        public int PlatoId { get; set; } 
        public string Nombre { get; set; } 
        public string Descripcion { get; set; } 
        public decimal Precio { get; set; } 
        public string Categoria { get; set; } 
        public bool Activo { get; set; } = true; 

        
        public ICollection<DetallePedido> DetallesPedido { get; set; }
    }
}
