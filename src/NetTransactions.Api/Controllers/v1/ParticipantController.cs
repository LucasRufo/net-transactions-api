using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using NetTransactions.Api.Domain.Request;
using NetTransactions.Api.Domain.Services;

namespace NetTransactions.Api.Controllers.v1;

[ApiController]
[ApiVersion(1)]
[Route("/api/v{version:apiVersion}/participant")]
public class ParticipantController : ControllerBase
{
    private readonly ParticipantService _participantService;

    public ParticipantController(ParticipantService participantService)
    {
        _participantService = participantService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await Task.FromResult("Hello World!"));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateParticipantRequest request)
    {
        var participant = await _participantService.Create(request);

        return Ok(participant.Value);
    }
}
