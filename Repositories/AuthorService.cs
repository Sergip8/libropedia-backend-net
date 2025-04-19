
using System.Data;
using Api.FunctionApp.DataContext;
using bookstore.Repositories.Interfaces;
using ConsultorioNet.Models.Request;
using ConsultorioNet.Models.Response;
using Dapper;

namespace bookstore.storeBackNet.Repositories
{
    public class AuthorService : IAuthorInterface
    {

        private readonly DapperContext _context;

        public AuthorService(DapperContext context)
        {
            _context = context;

        }

        public async Task<IEnumerable<FilterResponse>> FilterAuthors(FilterSearchRequest search)
        {
            using var connection = _context.CreateConnection();
            try
            {
                var query = @"
                   SELECT id_autor as Id, CONCAT(nombre, ' ', apellido) as Value
                FROM autores
                WHERE (LOWER(nombre) LIKE CONCAT('%', LOWER(@search), '%')
                    OR LOWER(apellido) LIKE CONCAT('%', LOWER(@search), '%')) 
                    ORDER BY nombre, apellido 
                    LIMIT @limit
                ";

                var result = await connection.QueryAsync<FilterResponse>(
                    query,
                    new { search = $"%{search.search}%", limit = search.limit }
                );
                return result;

            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving Category Info", ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }
    }

}