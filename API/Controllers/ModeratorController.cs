using Application.Interfaces;
using Common.Requests;
using Infrastructure.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/moderator")]
public class ModeratorController(IModeratorService moderatorService) : BaseApiController
{
    [HttpPost("update-transport-advertisement-status")]
    [HasPermission(Permissions.Moderator)]
    public async Task<IActionResult> UpdateAdvertisementStatus(UpdateAdvertisementStatusRequest request)
    {
        return HandleResult(
            await moderatorService.UpdateModeratorOverviewStatus(request));
    }

    [HttpGet("get-transport-advertisements/statusId={statusId}")]
    [HasPermission(Permissions.Moderator)]
    public async Task<IActionResult> GetTransportAdvertisementByStatusId(Guid statusId)
    {
        return HandleResult(await moderatorService.GetTransportAdvertisementByStatusId(statusId));
    }

    [HttpPost("update-transport-advertisement-verification-status")]
    [HasPermission(Permissions.Moderator)]
    public async Task<IActionResult> UpdateTransportAdvertisementVerificationStatus(UpdateTransportAdvertisementVerificationStatusRequest request)
    {
        return HandleResult(await moderatorService.UpdateTransportAdvertisementVerificationStatus(request));
    }
}