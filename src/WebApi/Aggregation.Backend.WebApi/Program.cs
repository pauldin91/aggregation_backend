using Aggregation.Backend.Application.Extensions;
using Aggregation.Backend.Infrastructure.Extensions;
using Aggregation.Backend.WebApi.Extensions;
using Aggregation.Backend.WebApi.Middlewares;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseSerilog(new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger());

builder.Services.AddApplicationExtensions();
builder.Services.AddInfrastructureExtensions(builder.Configuration);

builder.Services.AddWebApiExtensions();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseOutputCache();

app.UseMiddleware<RequestAnalyticsMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();