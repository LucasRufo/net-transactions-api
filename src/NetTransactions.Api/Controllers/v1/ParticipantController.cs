using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace NetTransactions.Api.Controllers.v1;

[ApiController]
[ApiVersion(1)]
[Route("/api/v{version:apiVersion}/participant")]
public class ParticipantController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await Task.FromResult("Hello World!"));
    }
}
