using LogConverterAPI.Data;
using LogConverterAPI.DTOs;
using LogConverterAPI.Repositorios;
using LogConverterAPI.Servicos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<ILogRepository, LogRepository>();

builder.Services.AddDbContext<LogContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

WebApplication app = builder.Build();

app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/logs/convertaLogFormatoAgora", async (ILogService logService, [FromBody] LogRequestDto request) =>
{
    if (string.IsNullOrEmpty(request.LogData))
    {
        return Results.BadRequest("Log não informado.");
    }

    LogResponseDto response = await logService.ConvertaLogParaFormatoAgora(request.LogData);
    return Results.Ok(response);
});

app.MapPost("/logs/insiraLog", async (ILogService logService, [FromBody] LogRequestDto request) =>
{
    if (string.IsNullOrEmpty(request.LogData))
    {
        return Results.BadRequest("Log não informado.");
    }

    await logService.InsiraLog(request);

    return Results.Ok();
});

app.MapGet("/obtenhaLogs", async (ILogService logService) =>
{
    IEnumerable<LogResponseDto> logs = await logService.ObtenhaTodosLogs();
    return Results.Ok(logs);
});

app.MapGet("/logs/{id}", async (ILogService logService, int id) =>
{
    LogResponseDto? log = await logService.ObtenhaLogPorId(id);
    return log == null ? Results.NotFound() : Results.Ok(log);
});

app.MapPost("/logs/salveEmArquivo", async (ILogService logService, [FromBody] LogRequestDto request) =>
{
    string filePath = await logService.SalveLogEmArquivo(request.LogData);
    return Results.Ok(filePath);
});

app.UseHttpsRedirection();
app.Run();
