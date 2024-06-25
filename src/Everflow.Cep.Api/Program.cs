using Everflow.Cep.Core;
using Everflow.Cep.Application;
using Everflow.Cep.Infrastructure.Persistence;
using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddAuthenticationJwtBearer(o => 
    o.SigningKey = "c73e933a293bec6f005080d0293b93e2c886ff4b9c49d4f666340c2a1170b007");

builder.Services.AddAuthorization();

builder.Services.AddFastEndpoints().SwaggerDocument();

builder.Services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddCore(builder.Configuration);

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication()
    .UseAuthorization();

app.UseDefaultExceptionHandler()
    .UseFastEndpoints(x => x.Errors.UseProblemDetails())
    .UseSwaggerGen();

app.Run();