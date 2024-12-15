using System.ComponentModel.DataAnnotations.Schema;

namespace LogConverterAPI.Entidades;

[Table("Logs")]
public class Log
{
    public int Id { get; set; }
    public string? LogFormatoCDN { get; set; }
    public string? LogFormatoAgora { get; set; }
}
