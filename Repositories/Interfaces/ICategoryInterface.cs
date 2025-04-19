
using ConsultorioNet.Models.Request;
using ConsultorioNet.Models.Response;

namespace bookstore.Repositories.Interfaces
{
    public interface ICategoryInterface
    {
    
        Task<IEnumerable<FilterResponse>> FilterCategory(FilterSearchRequest search);

      

    }
}