using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Platinum.Infrastructure.Data.Dapper
{
    public interface IDapperConnectionFactory
    {
        void CloseConnection();
        Task<DbConnection> CreateSqlConnection();
        void UseConnectionName(string connectionName);
    }

    public class DapperConnectionFactory : IDapperConnectionFactory
    {
        private readonly IConfiguration _configuration;

        public DapperConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private DbConnection _connection;

        private string connectionName { get; set; }

        public async Task<DbConnection> CreateSqlConnection()
        {
            if (string.IsNullOrEmpty(connectionName))
            {
                throw new NoNullAllowedException(nameof(connectionName));
            }

            if (_connection == null)
            {
                _connection = new SqlConnection(_configuration.GetConnectionString(connectionName));
            }

            if (_connection.State != ConnectionState.Open)
            {
                await _connection.OpenAsync();
            }

            return _connection;
        }

        public void CloseConnection()
        {
            if (_connection != null && _connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }

        public void UseConnectionName(string connectionName)
        {
            this.connectionName = connectionName;
        }
    }
}
