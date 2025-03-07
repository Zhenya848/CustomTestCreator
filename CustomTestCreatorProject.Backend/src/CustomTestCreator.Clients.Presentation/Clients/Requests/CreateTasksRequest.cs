using CustomTestCreator.Clients.Domain;
using CustomTestCreator.SharedKernel.ValueObjects.Dtos.ForQuery;
using Microsoft.AspNetCore.Http;

namespace CustomTestCreator.Clients.Presentation.Clients.Requests;

public record CreateTasksRequest(IEnumerable<CreateTaskDto> Tasks);