using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Almuerzos.Infrastructure.DTOs;

namespace Almuerzos.Infrastructure.DTOs
{
    public class ClienteReservasPorEstadoDto
    {
        public string Estado { get; set; }
        public int ClienteId { get; set; }
        public string NombreCliente { get; set; }
        public int TotalReservas { get; set; }
    }
}
