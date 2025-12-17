using System;
using System.Data;
using Almuerzos.Core.Enum;
using Almuerzos.Core.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Almuerzos.Infrastructure.Data
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public DatabaseProvider Provider => DatabaseProvider.SqlServer;

        public DbConnectionFactory(IConfiguration configuration)
        {
            // Intentar nombres comunes y usar fallback claro
            _connectionString = configuration.GetConnectionString("ConnectionSqlServer")
                                ?? configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                // Mensaje claro para debugging en lugar del error críptico de SqlConnection.Open()
                throw new InvalidOperationException(
                    "No se encontró ninguna cadena de conexión válida. " +
                    "Agrega 'ConnectionStrings:ConnectionSqlServer' o 'ConnectionStrings:DefaultConnection' en appsettings.json / User Secrets / variables de entorno."
                );
            }
        }

        public IDbConnection CreateConnection()
        {
            var connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;
        }
    }
}
