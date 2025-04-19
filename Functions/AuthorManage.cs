
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Functions.Worker;
using Consultorio.Function.Models;
using bookstore.Repositories.Interfaces;
using ConsultorioNet.Models.Request;
using Newtonsoft.Json;
namespace bookstore.storeBackNet.Functions
{
    public class AuthorManage
    {
        private readonly IAuthorInterface _authorInterface; 

          private readonly ILogger<UserManage> _logger;

        public AuthorManage(IAuthorInterface authorInterface, ILogger<UserManage> logger)
        {
            _authorInterface = authorInterface;
            _logger = logger;
        }


    [Function("GetAuthorFilter")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "GetAuthorFilter")] HttpRequest req)
    {
        
    
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        try{
             string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<FilterSearchRequest>(requestBody);
      
        if (data == null)
            {
                return new BadRequestObjectResult(FunctionsHelpers.CreateErrorResponse("Invalid request body."));
            }
      
   
        var res =  await _authorInterface.FilterAuthors(data);

        return new OkObjectResult(
            res
                );
            

        }catch(Exception ex){
             return new BadRequestObjectResult(new ResponseResult
                {
                    IsError = true,
                    Message = ex.Message,
                });
           
        }


    }
    


   
}   

}