using Application.Interfaces;
using Infrastructure.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/resource")]
public class ResourceController(IResourceService service) : BaseApiController
{
    [HttpGet("get-transport-types")]
    public async Task<IActionResult> GetTransportTypes()
    {
        return HandleResult(await service.GetTransportTypes());
    }

    [HttpGet("get-transport-makes/transportTypeId={transportTypeId}")]
    public async Task<IActionResult> GetTransportMakesByTransportTypeId(Guid transportTypeId)
    {
        return HandleResult(await service.GetTransportMakesByTransportTypeId(transportTypeId));
    }
    
    [HttpGet("get-transport-body-types/transportTypeId={transportTypeId}")]
    public async Task<IActionResult> GetTransportBodyTypesByTransportTypeId(Guid transportTypeId)
    {
        return HandleResult(await service.GetTransportBodyTypesByTransportTypeId(transportTypeId));
    }
    
    [HttpGet("get-transport-models/transportMakeId={transportMakeId}")]
    public async Task<IActionResult> GetTransportModelsByTransportMakeId(Guid transportMakeId)
    {
        return HandleResult(await service.GetTransportModelsByTransportMakeId(transportMakeId));
    }
    
    [HttpGet("get-moderator-overview-statuses")]
    public async Task<IActionResult> GetModeratorOverviewStatuses()
    {
        return HandleResult(await service.GetModeratorOverviewStatuses());
    }
}