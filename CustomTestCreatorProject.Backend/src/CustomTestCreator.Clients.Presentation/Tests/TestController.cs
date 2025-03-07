using CustomTestCreator.Clients.Application.Tests.Commands.Get;
using CustomTestCreator.Framework;
using Microsoft.AspNetCore.Mvc;

namespace CustomTestCreator.Clients.Presentation.Tests;

public class TestController : ApplicationController
{
    [HttpGet("{testId:guid}")]
    public async Task<ActionResult> GetById(
        [FromRoute] Guid testId,
        [FromServices] GetTestHandler handler,
        CancellationToken cancellationToken = default)
    {
        var petResult = await handler.Handle(testId, cancellationToken);

        if (petResult.IsFailure)
            return petResult.Error.ToResponse();
        
        return Ok(Envelope.Ok(petResult.Value));
    }
}