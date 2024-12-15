using LogConverterAPI.DTOs;

namespace LogConverterAPI.Servicos;

public interface ILogService
{
    Task<IEnumerable<LogResponseDto>> ObtenhaTodosLogs();
    Task<LogResponseDto?> ObtenhaLogPorId(int id);
    Task InsiraLog(LogRequestDto request);
    Task<LogResponseDto> ConvertaLogParaFormatoAgora(string logData);
    Task<string> SalveLogEmArquivo(string logData);
}
