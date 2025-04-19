
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using bookstore.storeBackNet.Repositories.Interfaces;
using Microsoft.Azure.Functions.Worker;
using ConsultorioNet.Models.Request;
using Consultorio.Function.Models;

namespace bookstore.storeBackNet.Functions
{
    public class UserManage
    {
        private readonly IUserInterface _userInterface;

          private readonly ILogger<UserManage> _logger;

        public UserManage(IUserInterface userInterface, ILogger<UserManage> logger)
        {
            _userInterface = userInterface;
            _logger = logger;
        }


    [Function("Register")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req)
    {
        
    
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        try{
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<RegisterRequest>(requestBody);
      
        
        if (string.IsNullOrEmpty(data?.Email) || string.IsNullOrEmpty(data?.Password))
        {
             return new BadRequestObjectResult(new ResponseResult
                {
                    IsError = true,
                    Message = "Please pass a username and password in the request body.",
                });
           
        }
        _logger.LogInformation(data.Email);
        await _userInterface.RegisterAsync(data);

        return new OkObjectResult(
            new ResponseResult
                {
                    IsError = false,
                    Message = "usuario registrado con exito",
                }
                );
            

        }catch(Exception ex){
             return new BadRequestObjectResult(new ResponseResult
                {
                    IsError = true,
                    Message = ex.Message,
                });
           
        }


    }

    [Function("Login")]
    public async Task<IActionResult> Login(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        try
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<LoginRequest>(requestBody);

            if (string.IsNullOrEmpty(data?.Email) || string.IsNullOrEmpty(data?.Password))
            {
                return new BadRequestObjectResult(new ResponseResult
                {
                    IsError = true,
                    Message = "Please pass a username and password in the request body.",
                });
            }

            _logger.LogInformation(data.ToString());
            var user = await _userInterface.LoginAsync(data);
            if (user == null)
            {
                return new UnauthorizedResult();
            }

           return new OkObjectResult(user);
        }
        catch (Exception ex)
        {
            return new BadRequestObjectResult(new ResponseResult
            {
                IsError = true,
                Message = ex.Message,
            });
        }
    }
}
}