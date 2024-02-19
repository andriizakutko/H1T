using Common.Results;

namespace Common;

public class Response
{
    public int StatusCode { get; set; }
    public object Data { get; set; }
    public Error Error { get; set; }
}