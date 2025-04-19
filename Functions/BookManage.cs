
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using bookstore.storeBackNet.Repositories.Interfaces;
using Microsoft.Azure.Functions.Worker;
using ConsultorioNet.Models.Request;
using Consultorio.Function.Models;
using bookstore.storeBackNet.Models.Request;
using bookstore.Repositories.Interfaces;

namespace bookstore.storeBackNet.Functions
{
    public class BookManage
    {
        private readonly IBookInterface _bookInterface ;

          private readonly ILogger<UserManage> _logger;

        public BookManage(IBookInterface bookInterface, ILogger<UserManage> logger)
        {
            _bookInterface = bookInterface;
            _logger = logger;
        }


    [Function("GetAllBooksPaginated")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req)
    {
        
    
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        try{
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<BookRequest>(requestBody);
      
        if (data == null)
            {
                return new BadRequestObjectResult(FunctionsHelpers.CreateErrorResponse("Invalid request body."));
            }
      
   
        var res =  await _bookInterface.FilterBookAsync(data);

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
    [Function("GetBookTopQualifications")]
    public async Task<IActionResult> GetBookTopQualifications(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route ="GetBookTopQualifications/{limit}")] HttpRequest req, int limit)
    {
        
    
        _logger.LogInformation("C# HTTP trigger function processed a request.");
     
       try{

      
   
        var res =  await _bookInterface.GetBookTopQualifications(limit);

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
 [Function("GetBookDetail")]
    public async Task<IActionResult> GetBookDetail(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetBookDetail/{id}")] HttpRequest req, long id)
    {
    
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        try{
   
        var res =  await _bookInterface.GetBookDetail(id);

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