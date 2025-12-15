using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Almuerzos.Core.Entities;
using Almuerzos.Core.Interfaces;
using Almuerzos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Dapper;
using Almuerzos.Core.QueryFilters;
using System;

namespace Almuerzos.Infrastructure.Repositories
{
    // Esta clase implementa IReservaRepository, que hereda de IRepository<Reserva>
    public class ReservaRepository : IReservaRepository
    {
        private readonly AlmuerzosDbContext _context;
        private readonly IDbConnectionFactory _connectionFactory;

        public ReservaRepository(AlmuerzosDbContext context, IDbConnectionFactory connectionFactory)
        {
            _context = context;
            _connectionFactory = connectionFactory;
        }

        // Consulta base con alias para Dapper
        private const string BaseReservaQuery = @"
            SELECT 
                r.reserva_id AS ReservaId, 
                r.cliente_id AS ClienteId, 
                r.horario_id AS HorarioId, 
                r.fecha_reserva AS FechaReserva,
                r.hora_solicitada AS HoraSolicitada,
                r.numero_personas AS NumeroPersonas,
                r.estado AS Estado,
                r.fecha_creacion AS FechaCreacion,

                c.cliente_id AS ClienteId, c.nombre, c.apellido, c.email, c.telefono, 
                
                h.horario_id AS HorarioId, 
                h.dia_semana AS DiaSemana, 
                h.hora_inicio AS HoraInicio, 
                h.hora_fin AS HoraFin, 
                h.capacidad_maxima AS CapacidadMaxima
            FROM Reservas r
            INNER JOIN Clientes c ON r.cliente_id = c.cliente_id
            INNER JOIN Horarios h ON r.horario_id = h.horario_id";


        // --- IMPLEMENTACIÓN DE IReservaRepository (Con Paginación y Filtros) ---

        public async Task<IEnumerable<Reserva>> GetReservas(ReservaQueryFilter filters)
        {
            // Use the existing implementation, but only return the IEnumerable<Reserva>
            var (reservas, _) = await GetReservasWithTotalCount(filters);
            return reservas;
        }

        // Rename the original GetReservas to avoid conflict and keep pagination support
        private async Task<(IEnumerable<Reserva> Reservas, int TotalCount)> GetReservasWithTotalCount(ReservaQueryFilter filters)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                // 1. Obtener el conteo total primero
                var totalCount = await GetTotalCount(filters);

                // 2. Definir los parámetros de Dapper
                var parameters = new DynamicParameters();

                // 3. Construir la consulta base (incluyendo filtrado)
                var sql = BaseReservaQuery;
                var whereClauses = new List<string>();

                // Aplicación de filtros
                if (!string.IsNullOrEmpty(filters.Estado))
                {
                    whereClauses.Add("r.estado = @Estado");
                    parameters.Add("@Estado", filters.Estado);
                }

                if (filters.ClienteId.HasValue)
                {
                    whereClauses.Add("r.cliente_id = @ClienteId");
                    parameters.Add("@ClienteId", filters.ClienteId.Value);
                }

                if (filters.FechaDesde.HasValue)
                {
                    whereClauses.Add("r.fecha_reserva >= @FechaDesde");
                    parameters.Add("@FechaDesde", filters.FechaDesde.Value);
                }

                if (filters.FechaHasta.HasValue)
                {
                    whereClauses.Add("r.fecha_reserva <= @FechaHasta");
                    parameters.Add("@FechaHasta", filters.FechaHasta.Value);
                }


                if (whereClauses.Any())
                {
                    sql += " WHERE " + string.Join(" AND ", whereClauses);
                }

                // 4. Aplicar Paginación (OFFSET / FETCH NEXT)
                sql += " ORDER BY r.reserva_id DESC"; // SQL Server requiere ORDER BY

                int skip = (filters.PageNumber - 1) * filters.PageSize;

                sql += $" OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY";

                parameters.Add("@Skip", skip);
                parameters.Add("@Take", filters.PageSize);


                var reservas = await connection.QueryAsync<Reserva, Cliente, Horario, Reserva>(
                    sql,
                    (reserva, cliente, horario) =>
                    {
                        reserva.Cliente = cliente;
                        reserva.Horario = horario;
                        return reserva;
                    },
                    parameters,
                    splitOn: "ClienteId,HorarioId"
                );

                return (reservas.ToList(), totalCount);
            }
        }

        // Método auxiliar para obtener el conteo total (necesario para la paginación)
        public async Task<int> GetTotalCount(ReservaQueryFilter filters)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                var parameters = new DynamicParameters();

                var countSql = "SELECT COUNT(r.reserva_id) FROM Reservas r";
                var whereClauses = new List<string>();

                // Aplicamos los mismos filtros que en GetReservas
                if (!string.IsNullOrEmpty(filters.Estado))
                {
                    whereClauses.Add("r.estado = @Estado");
                    parameters.Add("@Estado", filters.Estado);
                }

                if (filters.ClienteId.HasValue)
                {
                    whereClauses.Add("r.cliente_id = @ClienteId");
                    parameters.Add("@ClienteId", filters.ClienteId.Value);
                }

                if (filters.FechaDesde.HasValue)
                {
                    whereClauses.Add("r.fecha_reserva >= @FechaDesde");
                    parameters.Add("@FechaDesde", filters.FechaDesde.Value);
                }

                if (filters.FechaHasta.HasValue)
                {
                    whereClauses.Add("r.fecha_reserva <= @FechaHasta");
                    parameters.Add("@FechaHasta", filters.FechaHasta.Value);
                }

                if (whereClauses.Any())
                {
                    countSql += " WHERE " + string.Join(" AND ", whereClauses);
                }

                // Ejecutamos la consulta de conteo
                var totalCount = await connection.ExecuteScalarAsync<int>(countSql, parameters);

                return totalCount;
            }
        }

        // --- IMPLEMENTACIÓN DE IRepository<Reserva> ---

        public async Task<IEnumerable<Reserva>> GetAll()
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                var reservas = await connection.QueryAsync<Reserva, Cliente, Horario, Reserva>(
                  BaseReservaQuery,
                  (reserva, cliente, horario) =>
                  {
                      reserva.Cliente = cliente;
                      reserva.Horario = horario;
                      return reserva;
                  },
                  splitOn: "ClienteId,HorarioId"
                );

                return reservas.ToList();
            }
        }

        public async Task<Reserva> GetById(int id)
        {
            var sql = BaseReservaQuery + " WHERE r.reserva_id = @Id";

            using (var connection = _connectionFactory.CreateConnection())
            {
                var reservas = await connection.QueryAsync<Reserva, Cliente, Horario, Reserva>(
                  sql,
                  (reserva, cliente, horario) =>
                  {
                      reserva.Cliente = cliente;
                      reserva.Horario = horario;
                      return reserva;
                  },
                  param: new { Id = id },
                  splitOn: "ClienteId,HorarioId"
                );

                return reservas.FirstOrDefault();
            }
        }

        public async Task Add(Reserva reserva)
        {
            await _context.Reservas.AddAsync(reserva);
        }

        public async Task Delete(int id)
        {
            var currentReserva = await _context.Reservas.FindAsync(id);
            if (currentReserva == null) return;

            _context.Reservas.Remove(currentReserva);
        }

        public void Update(Reserva reserva)
        {
            _context.Reservas.Update(reserva);
        }

        // Tarea Clave: Contar personas en reservas NO CANCELADAS (Se mantiene con EF Core por la lógica)
        public async Task<int> GetReservasCountByHorarioAndDate(int horarioId, DateTime date)
        {
            var ocupacion = await _context.Reservas
             .Where(r =>
              r.horario_id == horarioId &&
              r.fecha_reserva.Date == date.Date &&
              r.estado != "Cancelada")
             .SumAsync(r => r.numero_personas);

            return ocupacion;
        }

        // --- Fix for CS0738: Change GetReservas to match interface signature ---
        // --- Fix for CS0535: Implement missing interface members ---

        // --- CS0535: Implement GetReserva(int) ---
        public async Task<Reserva> GetReserva(int id)
        {
            return await GetById(id);
        }

        // --- CS0535: Implement AddReserva(Reserva) ---
        public async Task AddReserva(Reserva reserva)
        {
            await Add(reserva);
        }

        // --- CS0535: Implement UpdateReserva(Reserva) ---
        public async Task<bool> UpdateReserva(Reserva reserva)
        {
            Update(reserva);
            await _context.SaveChangesAsync();
            return true;
        }

        // --- CS0535: Implement DeleteReserva(int) ---
        public async Task<bool> DeleteReserva(int id)
        {
            await Delete(id);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}