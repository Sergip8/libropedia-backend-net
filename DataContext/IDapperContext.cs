using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

public interface IDapperContext
{
    IDbConnection CreateConnection();
}