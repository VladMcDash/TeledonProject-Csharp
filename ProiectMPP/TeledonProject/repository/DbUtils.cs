using System.Data;
using Microsoft.Data.Sqlite;
using System.Configuration;
using log4net;

namespace ProiectMPP.TeledonProject.Repository
{
    public class DbUtils
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(DbUtils));
        private IDbConnection _connection = null;

        public IDbConnection GetConnection()
        {
            Log.Info("conectare db");
            if (_connection == null || _connection.State == ConnectionState.Closed)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["teledonDB"].ConnectionString;
                Log.DebugFormat("Connection string: {0}", connectionString);
                _connection = new SqliteConnection(connectionString);
                _connection.Open();
            }
            return _connection;
        }
    }
}