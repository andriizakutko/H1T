using System.Net;
using Common;
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
            ResultState.Ok => CreateResponse(HttpStatusCode.OK, result.Value),
            ResultState.NotFound => CreateResponse(HttpStatusCode.NotFound, result.Error),
            ResultState.BadRequest => CreateResponse(HttpStatusCode.BadRequest, result.Error),
            _ => CreateResponse(HttpStatusCode.InternalServerError, result.Error)
        };
    }
    
    protected static ActionResult HandleResult(Result result)
    {
        return result.State switch
        {
            ResultState.Ok => CreateResponse(HttpStatusCode.OK),
            ResultState.NotFound => CreateResponse(HttpStatusCode.NotFound, result.Error),
            ResultState.BadRequest => CreateResponse(HttpStatusCode.BadRequest, result.Error),
            _ => CreateResponse(HttpStatusCode.InternalServerError, result.Error)
        };
    }
    
    private static ActionResult CreateResponse(HttpStatusCode statusCode, object data = null, Error error = null)
    {
        var response = new Response()
        {
            StatusCode = (int)statusCode,
            Data = data,
            Error = error
        };

        return new OkObjectResult(response);
    }
}