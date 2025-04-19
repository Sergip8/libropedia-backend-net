
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Functions.Worker;
using Consultorio.Function.Models;
using bookstore.Repositories.Interfaces;
using ConsultorioNet.Models.Request;
using Newtonsoft.Json;
using bookstore.storeBackNet.Models.Request;
namespace bookstore.storeBackNet.Functions
{
    public class CommentManage
    {
        private readonly ICommentInterface _commentInterface;

        private readonly ILogger<UserManage> _logger;

        public CommentManage(ICommentInterface commentInterface, ILogger<UserManage> logger)
        {
            _commentInterface = commentInterface;
            _logger = logger;
        }


        [Function("StoreComment")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "StoreComment")] HttpRequest req, FunctionContext context)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<CommentRequest>(requestBody);
                var user = FunctionsHelpers.GetUserFromContext(context);
                if (user == null)
                {
                    return new UnauthorizedObjectResult(FunctionsHelpers.CreateErrorResponse("Invalid Token."));
                }

                if (data == null)
                {
                    return new BadRequestObjectResult(FunctionsHelpers.CreateErrorResponse("Invalid request body."));
                }

                var res = await _commentInterface.StoreComments(data);

                return new OkObjectResult(
                    res
                        );

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

        [Function("UpdateComment")]
        public async Task<IActionResult> UpdateComment(
                [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "UpdateComment")] HttpRequest req, FunctionContext context)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            try
            {
                var user = FunctionsHelpers.GetUserFromContext(context);
                if (user == null)
                {
                    return new UnauthorizedObjectResult(FunctionsHelpers.CreateErrorResponse("Invalid Token."));
                }
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<CommentUpdateRequest>(requestBody);

                if (data == null)
                {
                    return new BadRequestObjectResult(FunctionsHelpers.CreateErrorResponse("Invalid request body."));
                }

                var res = await _commentInterface.UpdateComments(data);

                return new OkObjectResult(
                    res
                        );

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
        [Function("GetUserComments")]
        public async Task<IActionResult> GetUserComments(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "GetUserComments")] HttpRequest req, FunctionContext context)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            try
            {
                var user = FunctionsHelpers.GetUserFromContext(context);
                if (user == null)
                {
                    return new UnauthorizedObjectResult(FunctionsHelpers.CreateErrorResponse("Invalid Token."));
                }
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<CommentUserRequest>(requestBody);

                if (data == null)
                {
                    return new BadRequestObjectResult(FunctionsHelpers.CreateErrorResponse("Invalid request body."));
                }

                var res = await _commentInterface.getUserComments(data);

                return new OkObjectResult(
                    res
                        );

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

        [Function("DeleteComment")]
        public async Task<IActionResult> GetBookTopQualifications(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "DeleteComment/{commentId}")] HttpRequest req, FunctionContext context, int commentId)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                var user = FunctionsHelpers.GetUserFromContext(context);
                if (user == null)
                {
                    return new UnauthorizedObjectResult(FunctionsHelpers.CreateErrorResponse("Invalid Token."));
                }
                var res = await _commentInterface.DeleteComment(commentId);

                return new OkObjectResult(
                    res
                        );

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