using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Almuerzos.Core.Enum; // Agregamos el using
using System.Threading.Tasks;

namespace Almuerzos.Core.Interfaces
{
    // Define la interfaz para crear y obtener una conexión de base de datos.
    // Incluye el proveedor para soportar múltiples tipos de bases de datos.
    public interface IDbConnectionFactory
    {
        DatabaseProvider Provider { get; } // Nueva propiedad
        IDbConnection CreateConnection();
    }
}
