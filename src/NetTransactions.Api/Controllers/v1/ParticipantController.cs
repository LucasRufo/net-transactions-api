using Asp.Versioning;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using NetTransactions.Api.Controllers.Shared;
using NetTransactions.Api.Domain.Request;
using NetTransactions.Api.Domain.Services;
using NetTransactions.Api.Extensions;
using System.Net;

namespace NetTransactions.Api.Controllers.v1;

[ApiController]
[ApiVersion(1)]
[Route("/api/v{version:apiVersion}/participant")]
public class ParticipantController : ControllerBase
{
    private readonly ParticipantService _participantService;
    private readonly IValidator<CreateParticipantRequest> _createParticipantValidator;

    public ParticipantController(ParticipantService participantService, IValidator<CreateParticipantRequest> validator)
    {
        _participantService = participantService;
        _createParticipantValidator = validator;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var participants = await _participantService.Get();

        return Ok(participants);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var participant = await _participantService.GetById(id);

        if (participant is null)
            return NotFound();

        return Ok(participant);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateParticipantRequest request)
    {
        var validationResult = await _createParticipantValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
            return BadRequest(new CustomProblemDetails(HttpStatusCode.BadRequest, Request.Path, validationResult.ToCustomProblemDetailsError()));

        var participant = await _participantService.Create(request);

        return Ok(participant.Value);
    }
}
