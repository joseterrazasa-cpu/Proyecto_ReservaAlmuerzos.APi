using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Almuerzos.Core.Entities;

namespace Almuerzos.Core.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio de lógica de negocio de Horarios.
    /// </summary>
    public interface IHorarioService
    {
        Task<IEnumerable<Horario>> GetHorarios();
        Task<Horario> GetHorario(int id);
        Task<bool> AddHorario(Horario horario);
        Task<bool> UpdateHorario(Horario horario);
        Task<bool> DeleteHorario(int id);
    }
}