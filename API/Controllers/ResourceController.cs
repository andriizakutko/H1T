using Application.Interfaces;
using Infrastructure.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/recource")]
public class ResourceController(IResourceService service) : BaseApiController
{
    [HttpGet("get-transport-types")]
    [HasPermission(Permissions.User)]
    public async Task<IActionResult> GetTransportTypes()
    {
        return HandleResult(await service.GetTransportTypes());
    }

    [HttpGet("get-transport-makes/{transportTypeId}")]
    [HasPermission(Permissions.User)]
    public async Task<IActionResult> GetTransportMakesByTransportTypeId(Guid transportTypeId)
    {
        return HandleResult(await service.GetTransportMakesByTransportTypeId(transportTypeId));
    }
    
    [HttpGet("get-transport-body-types/{transportTypeId}")]
    [HasPermission(Permissions.User)]
    public async Task<IActionResult> GetTransportBodyTypesByTransportTypeId(Guid transportTypeId)
    {
        return HandleResult(await service.GetTransportBodyTypesByTransportTypeId(transportTypeId));
    }
    
    [HttpGet("get-transport-models/{transportMakeId}")]
    [HasPermission(Permissions.User)]
    public async Task<IActionResult> GetTransportModelsByTransportMakeId(Guid transportMakeId)
    {
        return HandleResult(await service.GetTransportModelsByTransportMakeId(transportMakeId));
    }
    
    [HttpGet("get-moderator-overview-statuses")]
    [HasPermission(Permissions.User)]
    public async Task<IActionResult> GetModeratorOverviewStatuses()
    {
        return HandleResult(await service.GetModeratorOverviewStatuses());
    }
}