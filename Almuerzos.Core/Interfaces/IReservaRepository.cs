using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Almuerzos.Core.Entities;
using Almuerzos.Core.QueryFilters;

namespace Almuerzos.Core.Interfaces
{
    public interface IReservaRepository
    {
        // Modificado para aceptar el objeto de filtro/paginación
        // Retorna solo la página de Reservas.
        Task<IEnumerable<Reserva>> GetReservas(ReservaQueryFilter filters); 

        
        // Nuevo método para obtener el conteo total de registros (necesario para la paginación)
        Task<int> GetTotalCount(ReservaQueryFilter filters);

        Task<Reserva> GetReserva(int id);
        Task AddReserva(Reserva reserva);
        Task<bool> UpdateReserva(Reserva reserva);
        Task<bool> DeleteReserva(int id);
        Task<int> GetReservasCountByHorarioAndDate(int horarioId, DateTime date);
    }
}
