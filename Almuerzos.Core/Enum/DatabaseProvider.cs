using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Almuerzos.Core.Enum
{
    // Define los tipos de bases de datos que la aplicación puede usar.
    public enum DatabaseProvider
    {
        // Por ahora solo usamos SQLServer
        SqlServer = 1,
        // MySql = 2,
        // Oracle = 3
    }
}