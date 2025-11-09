using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Almuerzos.Core.Entities;

namespace Almuerzos.Core.Interfaces
{
    public interface IHorarioRepository
    {
        Task<IEnumerable<Horario>> GetHorariosByDay(int diaSemana);
        Task<Horario> GetHorarioById(int id);
        Task AddHorario(Horario horario);
        Task<bool> UpdateHorario(Horario horario);
        Task<bool> DeleteHorario(int id); 
    }
}
