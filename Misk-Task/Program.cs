using Misk_Task.Middleware;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;
var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
}); 


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapOpenApi();

app.UseSwagger(); 

app.UseSwaggerUI(); 

app.MapScalarApiReference();

app.UseMiddleware<RateLimitingMiddleware>();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
