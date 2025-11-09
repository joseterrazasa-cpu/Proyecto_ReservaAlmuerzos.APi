using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Almuerzos.Core.Exceptions
{
    // Esta excepción se lanzará cuando no haya espacio disponible.
    // Hereda de BusinessException para que nuestro filtro global sepa que es un error 400.
    public class CapacidadInsuficienteException : BusinessException
    {
        public CapacidadInsuficienteException()
        {
        }

        public CapacidadInsuficienteException(string message) : base(message)
        {
        }

        public CapacidadInsuficienteException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
