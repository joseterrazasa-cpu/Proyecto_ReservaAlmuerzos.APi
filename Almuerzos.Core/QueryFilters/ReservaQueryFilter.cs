using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Almuerzos.Core.QueryFilters
{
    // Clase que contendrá los parámetros de paginación y filtrado.
    public class ReservaQueryFilter
    {
        // --- Parámetros de Paginación ---
        public int PageNumber { get; set; } = 1; // Página por defecto
        public int PageSize { get; set; } = 10;  // Tamaño de página por defecto

        // --- Posibles Parámetros de Filtrado (Opcionales por ahora) ---
        public int? ClienteId { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public string? Estado { get; set; } // Ejemplo: "Confirmada", "Pendiente", "Cancelada"
    }
}
