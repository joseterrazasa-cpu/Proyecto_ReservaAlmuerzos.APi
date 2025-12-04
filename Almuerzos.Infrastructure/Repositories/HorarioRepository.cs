using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using Almuerzos.Core.Entities;
using Almuerzos.Core.Interfaces;
using Almuerzos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Dapper;

namespace Almuerzos.Infrastructure.Repositories
{
    public class HorarioRepository : IHorarioRepository
    {
        private readonly AlmuerzosDbContext _context;
        private readonly IDbConnectionFactory _connectionFactory;

        public HorarioRepository(AlmuerzosDbContext context, IDbConnectionFactory connectionFactory)
        {
            _context = context;
            _connectionFactory = connectionFactory;
        }

        
        public async Task<IEnumerable<Horario>> GetHorariosByDay(int diaSemana)
        {
            
            var sql = @"SELECT 
                            horario_id AS HorarioId, 
                            dia_semana AS DiaSemana, 
                            hora_inicio AS HoraInicio, 
                            hora_fin AS HoraFin, 
                            capacidad_maxima AS CapacidadMaxima
                        FROM Horarios 
                        WHERE dia_semana = @Dia";

            using (var connection = _connectionFactory.CreateConnection())
            {
                var horarios = await connection.QueryAsync<Horario>(sql, new { Dia = diaSemana });
                return horarios;
            }
        }

        
        public async Task<Horario> GetHorarioById(int id)
        {
            
            var sql = @"SELECT 
                            horario_id AS HorarioId, 
                            dia_semana AS DiaSemana, 
                            hora_inicio AS HoraInicio, 
                            hora_fin AS HoraFin, 
                            capacidad_maxima AS CapacidadMaxima
                        FROM Horarios 
                        WHERE horario_id = @Id";

            using (var connection = _connectionFactory.CreateConnection())
            {
                var horario = await connection.QueryFirstOrDefaultAsync<Horario>(sql, new { Id = id });
                return horario;
            }
        }

        public async Task<IEnumerable<Horario>> GetHorarios()
        {
            return await _context.Horarios.ToListAsync();
        }

        public async Task AddHorario(Horario horario)
        {
            await _context.Horarios.AddAsync(horario);
        }

        public async Task<bool> UpdateHorario(Horario horario)
        {
            _context.Horarios.Update(horario);
            return true;
        }

        public async Task<bool> DeleteHorario(int id)
        {
            var horario = await _context.Horarios.FindAsync(id);
            if (horario == null) return false;

            _context.Horarios.Remove(horario);
            return true;
        }
    }
}