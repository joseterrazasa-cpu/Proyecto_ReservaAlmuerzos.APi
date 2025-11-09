using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Almuerzos.Infrastructure.DTOs
{
    public class ClienteDto
    {
        public int ClienteId { get; set; }
        public string Nombre { get; set; }  
        public string Apellido { get; set; }    
        public string Email { get; set; }   
        public string Telefono { get; set; }
        public object NombreCompleto { get; internal set; }
    }
}
