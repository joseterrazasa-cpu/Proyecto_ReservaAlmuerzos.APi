using Almuerzos.Core.Entities;
using Almuerzos.Core.Interfaces;
using Almuerzos.Core.QueryFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Almuerzos.Core.Services
{
    public class ReservaService : IReservaService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReservaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // 1. Corregido: Tipo de retorno Task<bool>
        public async Task<Reserva> CrearReserva(Reserva nuevaReserva)
        {
            nuevaReserva.FechaCreacion = DateTime.Now;
            nuevaReserva.Estado = "Pendiente";

            await _unitOfWork.Reservas.AddReserva(nuevaReserva); // Cambiado de Add a AddReserva
            await _unitOfWork.SaveChangesAsync(); // Guardar cambios

            return nuevaReserva;
        }

        // 2. Implementación de Paginación y Filtros usando el Repositorio
        public async Task<(IEnumerable<Reserva> Reservas, int TotalCount)> GetReservas(ReservaQueryFilter filters)
        {
            var reservas = await _unitOfWork.Reservas.GetReservas(filters);
            var totalCount = await _unitOfWork.Reservas.GetTotalCount(filters);
            return (reservas, totalCount);
        }

        public async Task<Reserva> GetReserva(int id)
        {
            return await _unitOfWork.Reservas.GetReserva(id);
        }

        public async Task<bool> UpdateReserva(Reserva reserva)
        {
            var result = await _unitOfWork.Reservas.UpdateReserva(reserva);
            await _unitOfWork.SaveChangesAsync();
            return result;
        }

        public async Task<bool> CancelarReserva(int id)
        {
            var reserva = await _unitOfWork.Reservas.GetReserva(id);
            if (reserva == null) return false;

            reserva.Estado = "Cancelada";
            var result = await _unitOfWork.Reservas.UpdateReserva(reserva);
            await _unitOfWork.SaveChangesAsync();

            return result;
        }

        // 3. Método de lógica de negocio (Consultar Disponibilidad)
        public async Task<IEnumerable<Horario>> ConsultarDisponibilidad(DateTime fecha, int numeroPersonas)
        {
            // 1. Obtener el día de la semana (por ejemplo, Lunes, Martes, etc.)
            var diaSemana = fecha.DayOfWeek.ToString();

            // 2. Obtener todos los horarios disponibles para ese día
            var horarios = await _unitOfWork.Horarios.GetHorariosByDay(fecha.DayOfWeek.GetHashCode());

            var horariosDisponibles = new List<Horario>();

            foreach (var horario in horarios)
            {
                // 3. Contar el número de personas ya reservadas para ese horario y fecha (usando el método específico)
                var ocupacionActual = await _unitOfWork.Reservas.GetReservasCountByHorarioAndDate(horario.HorarioId, fecha);

                // 4. Calcular si hay capacidad
                if (horario.CapacidadMaxima - ocupacionActual >= numeroPersonas)
                {
                    horariosDisponibles.Add(horario);
                }
            }

            return horariosDisponibles;
        }
    }
}