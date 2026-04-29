using System.Data;
using Microsoft.Data.Sqlite;

namespace TeledonProject.Repository
{
    public class DbUtils
    {
        private readonly string _connectionString;

        public DbUtils(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection GetConnection()
        {
            return new SqliteConnection(_connectionString);
        }
    }
}