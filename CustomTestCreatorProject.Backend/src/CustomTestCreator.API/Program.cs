using CustomTestCreator.Accounts.Application;
using CustomTestCreator.Accounts.Implementation;
using CustomTestCreator.Accounts.Infrastructure;
using CustomTestCreator.Accounts.Infrastructure.Seeding;
using CustomTestCreator.API.Middleware;
using CustomTestCreator.Clients.Application;
using CustomTestCreator.Clients.Infrastructure;
using CustomTestCreator.Core.Infrastructure;
using CustomTestCreator.Framework;
using CustomTestCreator.TestHistories.Infrastructure;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyTestService", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Description = "JWT Authorization header {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{ }
        }
    });
});

builder.Services
    .AddFromCoreInfrastructure()
    .AddFromClientsInfrastructure(builder.Configuration)
    .AddFromAccountsApplication()
    .AddFromAccountsInfrastructure(builder.Configuration)
    .AddFromAccountsContract()
    .AddFromTestHistoriesInfrastructure()
    .AddFromClientsApplication()
    .AddFromFramework();

var app = builder.Build();

var accountsSeeder = app.Services.GetRequiredService<AccountsSeeder>();
await accountsSeeder.SeedAsync();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();