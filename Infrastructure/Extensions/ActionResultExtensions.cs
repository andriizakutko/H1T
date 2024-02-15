using System.Net;
using Domain.Results;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Extensions;

public static class ActionResultExtensions
{
    public static ActionResult HandleResult<T>(this ActionResult action, Result<T> result)
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
    
    public static ActionResult HandleResult(this ActionResult action, Result result)
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