using Application.Interfaces;
using Common.Requests;
using Infrastructure.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/moderator")]
public class ModeratorController(IModeratorService moderatorService) : BaseApiController
{
    [HttpGet("get-moderator-overview-statuses")]
    [HasPermission(Permissions.Moderator)]
    public async Task<IActionResult> GetModeratorOverviewStatuses()
    {
        return HandleResult(await moderatorService.GetModeratorOverviewStatuses());
    }

    [HttpPost("update-advertisement-status")]
    [HasPermission(Permissions.Moderator)]
    public async Task<IActionResult> UpdateAdvertisementStatus(UpdateAdvertisementStatusRequest request)
    {
        return HandleResult(
            await moderatorService.UpdateModeratorOverviewStatus(request));
    }

    [HttpGet("get-transport-advertisements/{statusId}")]
    [HasPermission(Permissions.Moderator)]
    public async Task<IActionResult> GetTransportAdvertisementByStatusId(Guid statusId)
    {
        return HandleResult(await moderatorService.GetTransportAdvertisementByStatusId(statusId));
    }
}