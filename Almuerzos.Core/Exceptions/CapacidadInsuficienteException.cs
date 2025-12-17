using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Almuerzos.Core.Exceptions
{
    
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
