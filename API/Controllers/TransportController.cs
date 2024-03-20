using Application.Interfaces;
using Infrastructure.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/transport")]
public class TransportController(ITransportService service) : BaseApiController
{
    [HttpGet("get-types")]
    [HasPermission(Permissions.User)]
    public async Task<IActionResult> GetTransportTypes()
    {
        return HandleResult(await service.GetTransportTypes());
    }

    [HttpGet("get-makes/{transportTypeId}")]
    [HasPermission(Permissions.User)]
    public async Task<IActionResult> GetTransportMakesByTransportTypeId(Guid transportTypeId)
    {
        return HandleResult(await service.GetTransportMakesByTransportTypeId(transportTypeId));
    }
    
    [HttpGet("get-body-types/{transportTypeId}")]
    [HasPermission(Permissions.User)]
    public async Task<IActionResult> GetTransportBodyTypesByTransportTypeId(Guid transportTypeId)
    {
        return HandleResult(await service.GetTransportBodyTypesByTransportTypeId(transportTypeId));
    }
    
    [HttpGet("get-models/{transportMakeId}")]
    [HasPermission(Permissions.User)]
    public async Task<IActionResult> GetTransportModelsByTransportMakeId(Guid transportMakeId)
    {
        return HandleResult(await service.GetTransportModelsByTransportMakeId(transportMakeId));
    }
}