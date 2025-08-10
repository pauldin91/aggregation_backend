using Aggregation.Backend.Application.Extensions;
using Aggregation.Backend.Infrastructure.Extensions;
using Aggregation.Backend.WebApi.Extensions;
using Aggregation.Backend.WebApi.Middlewares;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseSerilog(new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger());
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddApplicationExtensions();
builder.Services.AddInfrastructureExtensions(builder.Configuration);

builder.Services.AddWebApiExtensions();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<RequestAnalyticsMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseOutputCache();

app.UseAuthorization();

app.MapControllers();

app.Run();