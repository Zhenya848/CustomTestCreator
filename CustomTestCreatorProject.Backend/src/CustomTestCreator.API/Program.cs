using CustomTestCreator.API.Middleware;
using CustomTestCreator.Clients.Application;
using CustomTestCreator.Clients.Infrastructure;
using CustomTestCreator.Core.Infrastructure;
using CustomTestCreator.TestHistories.Infrastructure;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddFromCoreInfrastructure()
    .AddFromClientsInfrastructure(builder.Configuration)
    .AddFromTestHistoriesInfrastructure()
    .AddFromClientsApplication();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();