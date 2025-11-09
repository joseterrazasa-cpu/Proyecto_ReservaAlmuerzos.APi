using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Almuerzos.Core.Entities;
using Almuerzos.Core.QueryFilters;

namespace Almuerzos.Core.Interfaces
{
    public interface IReservaService
    {
        // Se cambia la firma para incluir el filtro y devolver la tupla (lista y conteo total).
        Task<(IEnumerable<Reserva> Reservas, int TotalCount)> GetReservas(ReservaQueryFilter filters);

        Task<Reserva> GetReserva(int id);

        // Se cambia la firma para devolver la entidad Reserva creada (que contiene el ID).
        Task<Reserva> CrearReserva(Reserva reserva);

        Task<bool> UpdateReserva(Reserva reserva);

        Task<bool> CancelarReserva(int id);
    }
}
