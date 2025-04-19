using bookstore.storeBackNet.Models.Request;
using bookstore.storeBackNet.Models.Response;
using ConsultorioNet.Models.Response;

namespace bookstore.Repositories.Interfaces
{
    public interface IBookInterface
    {
    
        Task<DataPaginatedResponse<BookResponse>> FilterBookAsync(BookRequest bookRequest);

        Task<IEnumerable<BookResponse>> GetBookTopQualifications(int limit);

        Task<BookDetailResponse> GetBookDetail(long id);


         
    }
}