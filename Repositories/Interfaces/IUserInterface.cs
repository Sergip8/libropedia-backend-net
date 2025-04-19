using Consultorio.Function.Models;
using ConsultorioNet.Models.Request;
using ConsultorioNet.Models.Response;


namespace bookstore.storeBackNet.Repositories.Interfaces
{
    public interface IUserInterface
    {
        Task<LoginResponse> LoginAsync(LoginRequest login);
        Task<ResponseResult> RegisterAsync(RegisterRequest register);
    }
}