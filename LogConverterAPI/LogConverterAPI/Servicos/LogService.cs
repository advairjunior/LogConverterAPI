using LogConverterAPI.DTOs;
using LogConverterAPI.Entidades;
using LogConverterAPI.Repositorios;
using LogConverterAPI.Servicos.Processos;

namespace LogConverterAPI.Servicos;

public class LogService(ILogRepository logRepository) : ILogService
{
    public async Task<IEnumerable<LogResponseDto>> ObtenhaTodosLogs()
    {
        IEnumerable<Log> logs = await logRepository.ObtenhaLogs();
        return logs.Select(log => new LogResponseDto(log.LogFormatoAgora ?? string.Empty, log.LogFormatoCDN ?? string.Empty));
    }

    public async Task<LogResponseDto?> ObtenhaLogPorId(int id)
    {
        Log? log = await logRepository.ObtenhaLogPorId(id);

        return log == null
            ? null
            : new LogResponseDto(log.LogFormatoAgora ?? string.Empty, log.LogFormatoCDN ?? string.Empty);
    }

    public async Task InsiraLog(LogRequestDto request)
    {
        Log log = new()
        {
            LogFormatoCDN = request.LogData,
            LogFormatoAgora = ProcessoDeConversaoDeLog.ConvertaLogParaFormatoAgora(request.LogData)
        };

        await logRepository.InsiraLog(log);
    }

    public Task<LogResponseDto> ConvertaLogParaFormatoAgora(string logData)
    {
        string logFormatoAgora = ProcessoDeConversaoDeLog.ConvertaLogParaFormatoAgora(logData);
        return Task.FromResult(new LogResponseDto(logData, logFormatoAgora));
    }

    public async Task<string> SalveLogEmArquivo(string logData)
    {
        string logFormatoAgora = ProcessoDeConversaoDeLog.ConvertaLogParaFormatoAgora(logData);

        string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "logsFormatoAgora");
        string filePath = Path.Combine(directoryPath, $"{Guid.NewGuid()}.txt");

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        await File.WriteAllTextAsync(filePath, logFormatoAgora);
        return filePath;
    }
}
