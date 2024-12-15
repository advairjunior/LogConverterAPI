using LogConverterAPI.Entidades;

namespace LogConverterAPI.Repositorios;
public interface ILogRepository
{
    Task<IEnumerable<Log>> ObtenhaLogs();
    Task<Log?> ObtenhaLogPorId(int id);
    Task InsiraLog(Log log);
    Task<IEnumerable<Log>> ObtenhaLogsTransformados();
}
