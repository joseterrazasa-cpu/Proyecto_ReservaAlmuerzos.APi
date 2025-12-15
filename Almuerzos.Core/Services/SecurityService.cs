using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Almuerzos.Core.Entities;
using Almuerzos.Core.Interfaces;
using System.Threading.Tasks;

namespace Almuerzos.Core.Services
{
    /// <summary>
    /// Implementación del servicio de seguridad.
    /// La verificación y el hashing de la contraseña se mueven a IPasswordService.
    /// </summary>
    public class SecurityService : ISecurityService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SecurityService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Busca el usuario solo por el login (nombre de usuario). 
        /// La verificación de la contraseña se hace en el TokenController.
        /// </summary>
        public async Task<Security> GetLoginByCredentials(UserLogin userLogin)
        {
            
            var user = await _unitOfWork.SecurityRepository.GetByLogin(userLogin.User);

            
            return user;
        }

        /// <summary>
        /// Registra un nuevo usuario de seguridad en la base de datos.
        /// La contraseña DEBE venir hasheada desde el SecurityController.
        /// </summary>
        public async Task RegisterUser(Security security)
        {
            await _unitOfWork.SecurityRepository.Add(security);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}