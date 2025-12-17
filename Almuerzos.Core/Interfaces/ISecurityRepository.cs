using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Almuerzos.Core.Entities;
using System.Threading.Tasks;

namespace Almuerzos.Core.Interfaces
{
    /// <summary>
    /// Interfaz específica del repositorio para la gestión de usuarios de seguridad.
    /// </summary>
    public interface ISecurityRepository
    {

        Task<Security> GetByLogin(string login);
        Task Add(Security entity);
    }
}
