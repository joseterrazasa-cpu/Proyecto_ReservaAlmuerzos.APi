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
        
        Task<(IEnumerable<Reserva> Reservas, int TotalCount)> GetReservas(ReservaQueryFilter filters);

        Task<Reserva> GetReserva(int id);

        
        Task<Reserva> CrearReserva(Reserva reserva);

        Task<bool> UpdateReserva(Reserva reserva);

        Task<bool> CancelarReserva(int id);
        Task<IEnumerable<(string Estado, Cliente Cliente, int TotalReservas)>> GetTopClientePorEstado();
    }
}
