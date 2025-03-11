using CustomTestCreator.Clients.Application.Clients.Commands.Create;
using CustomTestCreator.Clients.Application.Clients.Commands.Delete;
using CustomTestCreator.Clients.Application.Clients.Commands.Get;
using CustomTestCreator.Clients.Application.Clients.Commands.Update;
using CustomTestCreator.Clients.Application.Tasks.Commands.Create;
using CustomTestCreator.Clients.Application.Tasks.Commands.Delete;
using CustomTestCreator.Clients.Application.Tasks.Commands.UploadPhotos;
using CustomTestCreator.Clients.Application.Tests.Commands.Create;
using CustomTestCreator.Clients.Application.Tests.Commands.Delete;
using CustomTestCreator.Clients.Application.Tests.Commands.Update;
using CustomTestCreator.Clients.Presentation.Clients.Requests;
using CustomTestCreator.Framework;
using CustomTestCreator.SharedKernel.ValueObjects.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task = CustomTestCreator.Clients.Domain.Task;

namespace CustomTestCreator.Clients.Presentation.Clients;

public class ClientsController : ApplicationController
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromServices] CreateClientHandler handler,
        [FromBody] CreateClientRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateClientCommand(
            request.Name, 
            request.IsRandomTasks,
            request.IsInfiniteMode);
        
        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(Envelope.Ok(result.Value));
    }
    
    [HttpPut("{id:guid}/client-info")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromServices] UpdateClientHandler handler,
        [FromBody] UpdateClientRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateClientCommand(
            id,
            request.Name, 
            request.IsRandomTasks,
            request.IsInfiniteMode);
        
        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(Envelope.Ok(result.Value));
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult> Get(
        [FromRoute] Guid id,
        [FromServices] GetClientHandler handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(id, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(Envelope.Ok(result.Value));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid id,
        [FromServices] DeleteClientHandler handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(id, cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(Envelope.Ok(result.Value));
    }

    [HttpPost("{clientId:guid}/test")]
    public async Task<IActionResult> CreateTest(
        [FromRoute] Guid clientId,
        [FromServices] CreateTestHandler handler,
        [FromBody] CreateTestRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateTestCommand(
            clientId,
            request.TestName,
            request.Seconds,
            request.Minutes,
            request.Hours,
            request.IsTimeLimited,
            request.VerdictsList);
        
        var result = await handler.Handle(command, cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(Envelope.Ok(result.Value));
    }
    
    [HttpPut("{clientId:guid}/{testId:guid}/test")]
    public async Task<IActionResult> UpdateTest(
        [FromRoute] Guid clientId,
        [FromRoute] Guid testId,
        [FromServices] UpdateTestHandler handler,
        [FromBody] UpdateTestRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateTestCommand(
            testId,
            clientId,
            request.TestName,
            request.Seconds,
            request.Minutes,
            request.Hours,
            request.IsTimeLimited,
            request.VerdictsList);
        
        var result = await handler.Handle(command, cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(Envelope.Ok(result.Value));
    }

    [HttpDelete("{clientId:guid}/{testId:guid}/test")]
    public async Task<IActionResult> DeleteTest(
        [FromRoute] Guid clientId,
        [FromRoute] Guid testId,
        [FromServices] DeleteTestHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new DeleteTestCommand(clientId, testId);
        
        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(Envelope.Ok(result.Value));
    }
    
    [HttpPost("{clientId:guid}/{testId:guid}/task")]
    public async Task<IActionResult> CreateTask(
        [FromRoute] Guid clientId,
        [FromRoute] Guid testId,
        [FromServices] CreateTasksHandler handler,
        [FromBody] CreateTasksRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateTasksCommand(clientId, testId, request.Tasks);
        
        var result = await handler.Handle(command, cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(Envelope.Ok(result.Value));
    }

    
    [HttpPost("{clientId:guid}/{testId:guid}/task/photos")]
    public async Task<IActionResult> UploadPhotosToTasks(
        [FromRoute] Guid clientId,
        [FromRoute] Guid testId,
        [FromForm] IEnumerable<Guid> taskIds,
        [FromForm] IFormFileCollection files,
        [FromServices] UploadFilesToTasksHandler handler,
        CancellationToken cancellationToken = default)
    {
        await using FormFileProcessor formFileProcessor = new FormFileProcessor();
        List<UploadFileDto> fileDtos = formFileProcessor.StartProcess(files);
        
        var command = new UploadFilesToTasksCommand(clientId, testId, taskIds, fileDtos);
        
        var result = await handler.Handle(command, cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(Envelope.Ok(result.Value));
    }
    
    [HttpDelete("{clientId:guid}/{testId:guid}/task")]
    public async Task<IActionResult> DeleteTasks(
        [FromRoute] Guid clientId,
        [FromRoute] Guid testId,
        [FromForm] IEnumerable<Guid> taskIds,
        [FromServices] DeleteTasksHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new DeleteTasksCommand(clientId, testId, taskIds);
        
        var result = await handler.Handle(command, cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(Envelope.Ok(result.Value));
    }
}