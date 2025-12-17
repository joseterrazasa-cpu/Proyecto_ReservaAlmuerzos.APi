using Almuerzos.Core.Entities;
using Almuerzos.Core.Interfaces;
using Almuerzos.Core.QueryFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Almuerzos.Infrastructure.DTOs;


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
            nuevaReserva.fecha_creacion = DateTime.Now;
            nuevaReserva.estado = "Pendiente";

            await _unitOfWork.Reservas.AddReserva(nuevaReserva); 
            await _unitOfWork.SaveChangesAsync(); 

            return nuevaReserva;
        }

        
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

            reserva.estado = "Cancelada";
            var result = await _unitOfWork.Reservas.UpdateReserva(reserva);
            await _unitOfWork.SaveChangesAsync();

            return result;
        }

        
        public async Task<IEnumerable<Horario>> ConsultarDisponibilidad(DateTime fecha, int numeroPersonas)
        {
            
            var diaSemana = fecha.DayOfWeek.ToString();

            
            var horarios = await _unitOfWork.Horarios.GetHorariosByDay(fecha.DayOfWeek.GetHashCode());

            var horariosDisponibles = new List<Horario>();

            foreach (var horario in horarios)
            {
                
                var ocupacionActual = await _unitOfWork.Reservas.GetReservasCountByHorarioAndDate(horario.horario_id, fecha);

                
                if (horario.capacidad_maxima - ocupacionActual >= numeroPersonas)
                {
                    horariosDisponibles.Add(horario);
                }
            }

            return horariosDisponibles;
        }
        
        public async Task<IEnumerable<(string Estado, Cliente Cliente, int TotalReservas)>> GetTopClientePorEstado()
        {
            // Obtenemos la información agregada del repositorio (ya incluye ClienteId y NombreCliente)
            var data = await _unitOfWork.Reservas.GetTopClientePorEstado();

            // Construimos objetos Cliente ligeros con los datos ya disponibles en 'data'
            // Evitamos llamar a _unitOfWork.Clientes.GetCliente para no abrir una conexión por cada elemento.
            var result = data
                .Select(item => (
                    Estado: item.Estado,
                    Cliente: new Cliente
                    {
                        // Propiedades usadas por el controlador: cliente_id y nombre.
                        // Si tu entidad usa nombres diferentes, ajusta aquí.
                        cliente_id = item.ClienteId,
                        nombre = item.NombreCliente
                    },
                    TotalReservas: item.TotalReservas
                ))
                .ToList();

            return result;
        }
    }
}