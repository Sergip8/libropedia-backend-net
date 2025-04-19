
using System.Data;
using Api.FunctionApp.DataContext;
using bookstore.Repositories.Interfaces;
using ConsultorioNet.Models.Request;
using ConsultorioNet.Models.Response;
using Dapper;

namespace bookstore.storeBackNet.Repositories
{
    public class CategoryService : ICategoryInterface
    {

        private readonly DapperContext _context;

        public CategoryService(DapperContext context)
        {
            _context = context;

        }

        public async Task<IEnumerable<FilterResponse>> FilterCategory(FilterSearchRequest search)
{
    using var connection = _context.CreateConnection();
    try
    {
        var query = @"
            SELECT id_categoria AS Id, nombre AS Value 
            FROM categorias 
            WHERE nombre LIKE @search
            ORDER BY nombre
            ";

        var result = await connection.QueryAsync<FilterResponse>(
            query,
            new { search = $"%{search.search}%" }
        );

        return result;
    }
    catch (Exception ex)
    {
        throw new Exception("Error retrieving Category Info", ex);
    }
    finally
    {
        if (connection.State == ConnectionState.Open)
            connection.Close();
    }
}
    }

}