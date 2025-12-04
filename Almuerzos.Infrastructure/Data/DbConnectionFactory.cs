using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Almuerzos.Core.Interfaces;
using Almuerzos.Core.Enum; 
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Almuerzos.Infrastructure.Data
{
    
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        
        public DatabaseProvider Provider => DatabaseProvider.SqlServer;

        
        public DbConnectionFactory(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        
        public IDbConnection CreateConnection()
        {
            var connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;
        }
    }
}
