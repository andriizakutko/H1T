using Application.Interfaces;
using Common.Requests;
using Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/advertisement")]
public class AdvertisementController(ITransportAdvertisementService service) : BaseApiController
{
    [HttpGet("get-transport-advertisements")]
    [AllowAnonymous]
    public async Task<IActionResult> GetTransportAdvertisements()
    {
        return HandleResult(await service.GetTransportAdvertisements());
    }

    [HttpPost("create-transport-advertisement")]
    [HasPermission(Permissions.User)]
    public async Task<IActionResult> CreateTransportAdvertisement(CreateTransportAdvertisementRequest request)
    {
        return HandleResult(await service.CreateTransportAdvertisement(request));
    }
}