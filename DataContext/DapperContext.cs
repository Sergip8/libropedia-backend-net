using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace Api.FunctionApp.DataContext

{
    public class DapperContext: IDapperContext
    {
        public IDbConnection CreateConnection()
        {
            var connectionString = Environment.GetEnvironmentVariable("MySqlConnectionString");
         
            return new MySqlConnection(connectionString);
        }
    }
}