using LogConverterAPI.Data;
using LogConverterAPI.Entidades;
using Microsoft.EntityFrameworkCore;

namespace LogConverterAPI.Repositorios;

public class LogRepository(LogContext context) : ILogRepository
{
    public async Task<IEnumerable<Log>> ObtenhaLogs()
    {
        return await context.Logs.ToListAsync();
    }

    public async Task<Log?> ObtenhaLogPorId(int id)
    {
        return await context.Logs.FindAsync(id);
    }

    public async Task InsiraLog(Log log)
    {
        context.Logs.Add(log);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Log>> ObtenhaLogsTransformados()
    {
        return await context.Logs.Where(log => !string.IsNullOrEmpty(log.LogFormatoAgora))
                                 .ToListAsync();
    }
}
