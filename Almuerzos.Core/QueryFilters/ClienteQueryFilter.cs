using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Almuerzos.Core.Interfaces;

namespace Almuerzos.Infrastructure.DTOs.Filters
{
    /// <summary>
    /// Filtro para consultar entidades Cliente.
    /// Hereda de IQueryFilter para incluir paginación básica.
    /// </summary>
    public class ClienteQueryFilter 
    {
        /// <summary>
        /// Texto de búsqueda para filtrar por Nombre, Apellido o Email del cliente.
        /// </summary>
        public string? SearchTerm { get; set; }

        /// <summary>
        /// Número de página para la paginación.
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// Tamaño de la página para la paginación (cantidad de elementos).
        /// </summary>
        public int PageSize { get; set; } = 10;
    }
}
