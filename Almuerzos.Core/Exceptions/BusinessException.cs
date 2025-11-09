using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Almuerzos.Core.Exceptions
{
    // Esta será la clase base para todas las excepciones de Reglas de Negocio.
    // Usaremos esta clase para identificar qué excepciones son de tipo "400 Bad Request".
    public class BusinessException : Exception
    {
        public BusinessException()
        {
        }

        public BusinessException(string message) : base(message)
        {
        }

        public BusinessException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
