using Application.Interfaces;
using Common.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/advertisement")]
[AllowAnonymous]
public class AdvertisementController(IAdvertisementService service) : BaseApiController
{
    [HttpGet("get-transport-advertisement")]
    public async Task<IActionResult> GetTransportAdvertisements()
    {
        return HandleResult(await service.GetTransportAdvertisements());
    }

    [HttpPost("create-transport-advertisement")]
    public async Task<IActionResult> CreateTransportAdvertisement(CreateTransportAdvertisementRequest request)
    {
        return HandleResult(await service.CreateTransportAdvertisement(request));
    }
}