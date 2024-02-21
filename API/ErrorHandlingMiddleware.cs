using System.Net;
using Common;
using Common.Results;

namespace API;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        await _next(context);
        
        if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
        {
            context.Response.ContentType = "application/json";

            var response = new Response()
            {
                StatusCode = (int)HttpStatusCode.Unauthorized,
                Data = null,
                Error = new Error("Error.Unauthorized", "You are not authorized to work with this action")
            };
        
            await context.Response.WriteAsJsonAsync(response);
        }

        if (context.Response.StatusCode == (int)HttpStatusCode.Forbidden)
        {
            context.Response.ContentType = "application/json";

            var response = new Response()
            {
                StatusCode = (int)HttpStatusCode.Forbidden,
                Data = null,
                Error = new Error("Error.WrongPermissions", "You don't have permissions to work with this action")
            };
        
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}