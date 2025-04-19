
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
    public class CategoryManage
    {
        private readonly ICategoryInterface _categoryInterface ;

          private readonly ILogger<UserManage> _logger;

        public CategoryManage(ICategoryInterface categoryInterface, ILogger<UserManage> logger)
        {
            _categoryInterface = categoryInterface;
            _logger = logger;
        }


    [Function("GetCategoryFilter")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "GetCategoryFilter")] HttpRequest req)
    {
        
    
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        try{
              string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<FilterSearchRequest>(requestBody);
      
        if (data == null)
            {
                return new BadRequestObjectResult(FunctionsHelpers.CreateErrorResponse("Invalid request body."));
            }
      
   
   
        var res =  await _categoryInterface.FilterCategory(data);

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