using System.Data;
using System.Threading.Tasks;
using Api.FunctionApp.DataContext;
using bookstore.storeBackNet.DataContext;
using bookstore.storeBackNet.Repositories.Interfaces;
using Consultorio.Function.Models;
using ConsultorioNet.Models.Request;
using ConsultorioNet.Models.Response;
using Dapper;
using EventManagementSystem.Helpers;

namespace bookstore.storeBackNet.Repositories
{
    public class UserService : IUserInterface
    {

        private readonly IDapperContext _context;
         private readonly IDapperWrapper _wrapper;
        private readonly JwtSettings _jwtSettings;
        public UserService(IDapperContext context, JwtSettings jwtSettings, IDapperWrapper wrapper)
    {
        _context = context;
        _jwtSettings = jwtSettings;
         _wrapper = wrapper;
    }

        async public Task<LoginResponse> LoginAsync(LoginRequest login)
    {
    using var connection = _context.CreateConnection();
    var parameters = new DynamicParameters();
    parameters.Add("p_email", login.Email, DbType.String);
    parameters.Add("p_password", login.Password, DbType.String);

    var result = await _wrapper.QueryFirstOrDefaultAsync<UserResponse>(
        connection,
        "sp_autenticar_usuario", 
        parameters, 
        commandType: CommandType.StoredProcedure
    );

    
    string token = JwtHelper.GenerateJwt(_jwtSettings, result.Id, result.Email, result.Username);
    return new LoginResponse {
           Token = token,
           Message = "Login successful",
           IsError = false,
           User = result
    };
    }

    async public Task<ResponseResult> RegisterAsync(RegisterRequest register)
    {
        using var connection = _context.CreateConnection();
        var parameters = new DynamicParameters();
        parameters.Add("p_email", register.Email, DbType.String);
        parameters.Add("p_password", register.Password, DbType.String);
        parameters.Add("p_nombre_usuario", register.Username, DbType.String);

        parameters.Add("p_rol", "CUSTOMER", DbType.String);

        var result = await _wrapper.ExecuteAsync(connection,"sp_registrar_usuario", parameters, commandType: CommandType.StoredProcedure);

        return new ResponseResult
        {
            IsError = result < 0,
            Message = result > 0 ? "User registered successfully" : "User registration failed"
        };
    }

      
    }
}