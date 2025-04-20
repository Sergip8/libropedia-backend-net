using System.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using Dapper;

namespace bookstore.storeBackNet.DataContext
{
    public interface IDapperWrapper
    {
        Task<IEnumerable<T>> QueryAsync<T>(IDbConnection connection, string sql, object parameters = null, CommandType? commandType = null);
    Task<int> ExecuteAsync(IDbConnection connection, string sql, object parameters = null, CommandType? commandType = null);
    

 Task<T> QueryFirstOrDefaultAsync<T>(
        IDbConnection connection,
        string sql,
        object parameters = null,
        CommandType? commandType = null);

    
        
    
    }
}
