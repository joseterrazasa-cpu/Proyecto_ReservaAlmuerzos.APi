using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Almuerzos.Infrastructure.DTOs
{
    public class CrearReservaDto
    {
        
        public DateTime FechaReserva { get; set; }
        public TimeSpan HoraSolicitada { get; set; }
        public int NumeroPersonas { get; set; }

         
        public int? ClienteId { get; set; }

        
        public string NuevoClienteNombre { get; set; }
        public string NuevoClienteApellido { get; set; }
        public string NuevoClienteEmail { get; set; } 
        public string NuevoClienteTelefono { get; set; } 
    }
}
