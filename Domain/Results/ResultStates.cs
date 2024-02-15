namespace Domain.Results;

public enum ResultState
{
    Ok, 
    BadRequest,
    UnprocessableEntity,
    NotFound,
    Unauthorized
}