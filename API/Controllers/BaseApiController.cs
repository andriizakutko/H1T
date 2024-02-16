using System.Net;
using Common.Results;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
public class BaseApiController : ControllerBase
{
    protected static ActionResult HandleResult<T>(Result<T> result)
    {
        return result.State switch
        {
            ResultState.Ok => new OkObjectResult(result.Value),
            ResultState.NotFound => CreateResponse(HttpStatusCode.NotFound, result.Error),
            ResultState.BadRequest => new BadRequestObjectResult(result.Error),
            ResultState.Unauthorized => new UnauthorizedObjectResult(result.Error),
            _ => CreateResponse(HttpStatusCode.InternalServerError, result.Error)
        };
    }
    
    protected static ActionResult HandleResult(Result result)
    {
        return result.State switch
        {
            ResultState.Ok => CreateResponse(HttpStatusCode.OK),
            ResultState.NotFound => CreateResponse(HttpStatusCode.NotFound, result.Error),
            ResultState.BadRequest => new BadRequestObjectResult(result.Error),
            ResultState.Unauthorized => new UnauthorizedObjectResult(result.Error),
            _ => CreateResponse(HttpStatusCode.InternalServerError, result.Error)
        };
    }
    
    private static ActionResult CreateResponse(HttpStatusCode statusCode, Error error = null)
    {
        var response = new ObjectResult(error)
        {
            StatusCode = (int)statusCode
        };

        return response;
    }
}