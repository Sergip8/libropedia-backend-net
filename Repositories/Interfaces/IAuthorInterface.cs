
using ConsultorioNet.Models.Request;
using ConsultorioNet.Models.Response;

namespace bookstore.Repositories.Interfaces
{
    public interface IAuthorInterface
    {
    
        Task<IEnumerable<FilterResponse>> FilterAuthors(FilterSearchRequest search);

      

    }
}