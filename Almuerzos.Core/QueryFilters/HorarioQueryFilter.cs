using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Almuerzos.Core.Interfaces;

namespace Almuerzos.Infrastructure.DTOs.Filters
{
    /// <summary>
    /// Filtro para consultar entidades Horario.
    /// Hereda de IQueryFilter para incluir paginación básica.
    /// </summary>
    public class HorarioQueryFilter
    {
        /// <summary>
        /// Filtra horarios con una capacidad máxima igual o superior al valor especificado.
        /// </summary>
        public int? MinCapacidad { get; set; }

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
