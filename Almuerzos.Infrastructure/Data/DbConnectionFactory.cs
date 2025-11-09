using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Almuerzos.Core.Interfaces;
using Almuerzos.Core.Enum; // Agregamos el using
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Almuerzos.Infrastructure.Data
{
    // Implementación de la factoría que crea una conexión SQL Server.
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        // Propiedad requerida por la interfaz, indica que esta fábrica es para SQL Server.
        public DatabaseProvider Provider => DatabaseProvider.SqlServer;

        // Inyectamos IConfiguration para obtener el ConnectionString
        public DbConnectionFactory(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Crea y devuelve una nueva conexión abierta de SQL Server.
        public IDbConnection CreateConnection()
        {
            var connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;
        }
    }
}
