using System.Data;
using Api.FunctionApp.DataContext;
using bookstore.Repositories.Interfaces;
using bookstore.storeBackNet.DataContext;
using bookstore.storeBackNet.Models.Request;
using bookstore.storeBackNet.Models.Response;
using Consultorio.Function.Models;
using ConsultorioNet.Models.Response;
using Dapper;

namespace bookstore.storeBackNet.Repositories
{
    public class CommentService : ICommentInterface
    {

        private readonly IDapperContext _context;
       private readonly IDapperWrapper _wrapper;
        public CommentService(IDapperContext context, IDapperWrapper wrapper)
    {
        _context = context;
        _wrapper = wrapper;
  
    }

        public async Task<ResponseResult> StoreComments(CommentRequest comment)
        {
            
            using var connection = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("p_id_libro", comment.IdLibro, DbType.String);
            parameters.Add("p_id_usuario", comment.IdUsuario, DbType.Int32);
            parameters.Add("p_calificacion", comment.Calificacion, DbType.Int32);
            parameters.Add("p_comentario", comment.Comentario, DbType.String);

            var result = await _wrapper.ExecuteAsync(
                connection,
                "sp_agregar_resena",
                parameters,
                commandType: CommandType.StoredProcedure
            );
             return new ResponseResult
        {
            IsError = result < 0,
            Message = result > 0 ? "Comentario agregado" : "No es posible agregar el comentario"
        };
        }

         public async Task<ResponseResult> UpdateComments(CommentUpdateRequest comment)
        {
            
            using var connection = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("p_id_resena", comment.IdResena, DbType.Int32);
            parameters.Add("p_id_usuario", comment.IdUsuario, DbType.Int32);
            parameters.Add("p_nuevo_comentario", comment.Comentario, DbType.String);
            parameters.Add("p_nueva_calificacion", comment.Calificacion, DbType.Int32);

            var result = await _wrapper.ExecuteAsync(
                connection,
                "sp_actualizar_resena",
                parameters,
                commandType: CommandType.StoredProcedure
            );
             return new ResponseResult
        {
            IsError = result < 0,
            Message = result > 0 ? "Comentario modificado" : "No es posible modificar el comentario"
        };
        }
         public async Task<DataPaginatedResponse<CommentUserResponse>> getUserComments(CommentUserRequest comment)
        {
            var result = new DataPaginatedResponse<CommentUserResponse>{};
            using var connection = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("p_id_usuario", comment.UserId, DbType.Int32);
            parameters.Add("p_offset", comment.Offset, DbType.Int32);
            parameters.Add("p_limite", comment.Limit, DbType.Int32);

            using var multi = await connection.QueryMultipleAsync("sp_obtener_resenas_usuario", parameters, commandType: CommandType.StoredProcedure);
             
            result.Data = (await multi.ReadAsync<CommentUserResponse>()).ToList();
            result.TotalRecords = (await multi.ReadAsync<int>()).FirstOrDefault();

            return result;
        }
    
     public async Task<ResponseResult> DeleteComment(int commentId)
{

    using var connection = _context.CreateConnection();
    try
    {
        var query = @"
            DELETE  
            FROM resenas 
            WHERE id_resena = @CommentId 
            ";

        var affectedRows = await _wrapper.ExecuteAsync(
            connection,
            query,
            new { CommentId = commentId } 
        );

        if (affectedRows > 0)
        {
            return new ResponseResult
            {
                IsError = false,
                Message = "Reseña eliminada"
            };
        }
        else
        {
            // No rows affected means the comment ID wasn't found
            return new ResponseResult
            {
                IsError = true, // It's an error/issue if the comment wasn't found for deletion
                Message = $"Reseña con ID {commentId} no encontrada."
            };
        }
    }
    catch (Exception ex)
    {
       
        throw; 
    }
   
}
    }
    }