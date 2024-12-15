using System.Text;

namespace LogConverterAPI.Servicos.Processos;

public class ProcessoDeConversaoDeLog
{
    public static string ConvertaLogParaFormatoAgora(string logData)
    {
        string[] linhas = logData.Split(["\r\n", "\n"], StringSplitOptions.None);
        List<string> linhasConvertidas = [];

        foreach (string linha in linhas)
        {
            string[] partes = linha.Split('|');

            if (partes.Length == 5)
            {
                string provider = "\"MINHA CDN\"";
                (string httpMethod, string uriPath) = ObtenhaMetodoEUri(partes[3]);
                string statusCode = partes[1];
                string cacheStatus = AjusteCacheStatus(partes[2]);

                double responseSize = double.Parse(partes[4], System.Globalization.CultureInfo.InvariantCulture);
                string responseSizeArredondado = Math.Round(responseSize).ToString("0");

                string responseTime = partes[0];

                string linhaConvertida = new StringBuilder().Append(provider)
                                                            .Append(' ')
                                                            .Append(httpMethod)
                                                            .Append(' ')
                                                            .Append(statusCode)
                                                            .Append(' ')
                                                            .Append(uriPath)
                                                            .Append(' ')
                                                            .Append(responseSizeArredondado)
                                                            .Append(' ')
                                                            .Append(responseTime)
                                                            .Append(' ')
                                                            .Append(cacheStatus)
                                                            .ToString();

                linhasConvertidas.Add(linhaConvertida);
            }
        }

        return string.Join("\n", linhasConvertidas);
    }

    private static (string httpMethod, string uriPath) ObtenhaMetodoEUri(string methodWithUri)
    {
        var parts = methodWithUri.Trim('"').Split(' ');
        return (parts[0], parts[1]);
    }

    private static string AjusteCacheStatus(string cacheStatus)
    {
        return cacheStatus == "INVALIDATE" ? "REFRESH_HIT" : cacheStatus;
    }
}
