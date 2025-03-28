using CustomTestCreator.Clients.Application.Tests.Commands.Get;
using CustomTestCreator.Clients.Application.Tests.Queries;
using CustomTestCreator.Clients.Presentation.Tests.Requests;
using CustomTestCreator.Framework;
using CustomTestCreator.Framework.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomTestCreator.Clients.Presentation.Tests;

public class TestController : ApplicationController
{
    [HttpGet("{testId:guid}")]
    [Permission("test.get")]
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

    [HttpGet("tests")]
    [Permission("tests.get")]
    public async Task<ActionResult> GetTestsByPagination(
        [FromQuery] GetTestsWithPaginationRequest request,
        [FromServices] GetTestsWithPaginationHandler handler,
        CancellationToken cancellationToken = default)
    {
        var query = new GetTestsWithPaginationQuery(request.Page, request.PageSize);
        
        var result = await handler.Handle(query, cancellationToken);
        
        return Ok(Envelope.Ok(result));
    }
}