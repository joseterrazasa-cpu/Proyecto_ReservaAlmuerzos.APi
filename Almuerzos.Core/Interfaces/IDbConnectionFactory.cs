using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Almuerzos.Core.Enum; 
using System.Threading.Tasks;

namespace Almuerzos.Core.Interfaces
{
    
    public interface IDbConnectionFactory
    {
        DatabaseProvider Provider { get; } // Nueva propiedad
        IDbConnection CreateConnection();
    }
}
