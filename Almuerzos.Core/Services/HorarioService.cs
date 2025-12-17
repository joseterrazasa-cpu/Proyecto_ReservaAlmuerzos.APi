using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Almuerzos.Core.Entities;
using Almuerzos.Core.Interfaces;
using Almuerzos.Core.Exceptions;

namespace Almuerzos.Core.Services
{
    public class HorarioService : IHorarioService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HorarioService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Horario>> GetHorarios()
        {
            
            var horarios = new List<Horario>();
            for (int diaSemana = 0; diaSemana <= 6; diaSemana++)
            {
                var horariosPorDia = await _unitOfWork.Horarios.GetHorariosByDay(diaSemana);
                horarios.AddRange(horariosPorDia);
            }
            return horarios;
        }

        public async Task<Horario> GetHorario(int id)
        {
            return await _unitOfWork.Horarios.GetHorarioById(id);
        }

        public async Task<bool> AddHorario(Horario horario)
        {
            
            if (horario.hora_inicio >= horario.hora_fin)
            {
                throw new BusinessException("La hora de inicio debe ser anterior a la hora de fin.");
            }

            await _unitOfWork.Horarios.AddHorario(horario);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateHorario(Horario horario)
        {
            var horarioActual = await _unitOfWork.Horarios.GetHorarioById(horario.horario_id);
            if (horarioActual == null)
            {
                return false; 
            }

            
            if (horario.hora_inicio >= horario.hora_fin)
            {
                throw new BusinessException("La hora de inicio debe ser anterior a la hora de fin.");
            }

            
            horarioActual.hora_inicio = horario.hora_inicio;
            horarioActual.hora_fin = horario.hora_fin;
            horarioActual.capacidad_maxima = horario.capacidad_maxima;
            horarioActual.Descripcion = horario.Descripcion;

            _unitOfWork.Horarios.UpdateHorario(horarioActual);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteHorario(int id)
        {
            
            var resultado = await _unitOfWork.Horarios.DeleteHorario(id);
            await _unitOfWork.SaveChangesAsync();
            return resultado;
        }
    }
}
