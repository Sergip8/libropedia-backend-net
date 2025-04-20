using System.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using Dapper;

namespace bookstore.storeBackNet.DataContext
{
    public class DapperWrapper : IDapperWrapper
    {


      public Task<IEnumerable<T>> QueryAsync<T>(IDbConnection connection, string sql, object parameters = null, CommandType? commandType = null)
    {
        return connection.QueryAsync<T>(sql, parameters, commandType: commandType);
    }

    public Task<int> ExecuteAsync(IDbConnection connection, string sql, object parameters = null, CommandType? commandType = null)
    {
        return connection.ExecuteAsync(sql, parameters, commandType: commandType);
    }
     public Task<T> QueryFirstOrDefaultAsync<T>(IDbConnection connection, string sql, object parameters = null, CommandType? commandType = null)
    {
        return connection.QueryFirstOrDefaultAsync<T>(sql, parameters, commandType: commandType);
    }

       

  
    }
}
