
using System.Data;
using Api.FunctionApp.DataContext;
using bookstore.Repositories.Interfaces;
using bookstore.storeBackNet.DataContext;
using ConsultorioNet.Models.Request;
using ConsultorioNet.Models.Response;
using Dapper;

namespace bookstore.storeBackNet.Repositories
{
    public class CategoryService : ICategoryInterface
    {

        private readonly IDapperContext _context;
        private readonly IDapperWrapper _wrapper;
        public CategoryService(IDapperContext context, IDapperWrapper wrapper)
        {
            _context = context;
            _wrapper = wrapper;

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

        var result = await _wrapper.QueryAsync<FilterResponse>(
            connection,
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