
using bookstore.storeBackNet.Models.Request;
using bookstore.storeBackNet.Models.Response;
using Consultorio.Function.Models;
using ConsultorioNet.Models.Request;
using ConsultorioNet.Models.Response;

namespace bookstore.Repositories.Interfaces
{
    public interface ICommentInterface
    {
    
        Task<ResponseResult> StoreComments(CommentRequest comment);

        Task<ResponseResult> UpdateComments(CommentUpdateRequest comment);
        Task<DataPaginatedResponse<CommentUserResponse>> getUserComments(CommentUserRequest comment);
        Task<ResponseResult> DeleteComment(int commentId);

    }
}