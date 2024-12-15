using LogConverterAPI.Tests.DTOs;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace LogConverterAPI.Tests;

public class LogConverterAPITest
{
    private readonly HttpClient _client;

    public LogConverterAPITest()
    {
        _client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7294")
        };
    }

    [Fact]
    public async Task Deve_Converter_Log_Formato_CND_Para_Formato_Agora()
    {
        //arrange
        string logData = ObtenhaLogFormatoCND();
        string logFormatoAgora = ObtenhaLogFormatoAgora();

        LogRequestDto request = new(logData);
        var requestContent = new StringContent(JsonConvert.SerializeObject(request), System.Text.Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/logs/convertaLogFormatoAgora", requestContent);

        // Assert
        response.EnsureSuccessStatusCode();

        var logResponse = await response.Content.ReadFromJsonAsync<LogResponseDto>();

        Assert.Equal(logData, logResponse.LogFormatoCDN);
        Assert.Equal(logFormatoAgora, logResponse.LogFormatoAgora);
    }

    [Fact]
    public async Task Deve_Inserir_Log_No_Banco()
    {
        //arrange
        string logData = ObtenhaLogFormatoCND();

        LogRequestDto request = new(logData);
        var requestContent = new StringContent(JsonConvert.SerializeObject(request), System.Text.Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/logs/insiraLog", requestContent);

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task Deve_Obter_Logs_Salvos_No_Banco()
    {
        // Act
        var response = await _client.GetAsync("/obtenhaLogs");
        response.EnsureSuccessStatusCode();

        // Assert
        var logResponse = await response.Content.ReadFromJsonAsync<List<LogResponseDto>>();
        Assert.NotNull(logResponse);
        Assert.NotEmpty(logResponse);
        Assert.All(logResponse, ValideLog);
    }

    [Fact]
    public async Task Deve_Obter_Log_Salvo_Por_Id()
    {
        // Arrange
        int logId = 1;

        // Act
        var response = await _client.GetAsync($"/logs/{logId}");
        response.EnsureSuccessStatusCode();

        // Assert
        var logResponse = await response.Content.ReadFromJsonAsync<LogResponseDto>();
        ValideLog(logResponse);
    }

    [Fact]
    public async Task Deve_Retornar_NotFound_Para_Log_Inexistente()
    {
        // Arrange
        int logIdInexistente = 9999;

        // Act
        var response = await _client.GetAsync($"/logs/{logIdInexistente}");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Deve_Salvar_Log_Em_Arquivo()
    {
        // Arrange
        string logData = ObtenhaLogFormatoCND();

        var request = new LogRequestDto(logData);
        var requestContent =
            new StringContent(JsonConvert.SerializeObject(request), System.Text.Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/logs/salveEmArquivo", requestContent);
        response.EnsureSuccessStatusCode();

        // Assert
        var filePath = await response.Content.ReadFromJsonAsync<string>();
        Assert.NotNull(filePath);
        Assert.True(File.Exists(filePath), $"O arquivo não foi salvo no caminho esperado: {filePath}");

        File.Delete(filePath);
    }

    private static void ValideLog(LogResponseDto log)
    {
        Assert.False(string.IsNullOrWhiteSpace(log.LogFormatoCDN), "Log formato \"CDN\" não pode ser nulo ou vazio.");
        Assert.False(string.IsNullOrWhiteSpace(log.LogFormatoAgora), "Log formato \"agora\" não pode ser nulo ou vazio.");
    }

    private static string ObtenhaLogFormatoAgora()
    {
        return "\"MINHA CDN\" GET 200 /robots.txt 100 312 HIT\n\"MINHA CDN\" POST 200 /myImages 319 101 MISS\n\"MINHA CDN\" GET 404 /not-found 143 199 MISS\n\"MINHA CDN\" GET 200 /robots.txt 245 312 REFRESH_HIT";
    }

    private static string ObtenhaLogFormatoCND()
    {
        return "312|200|HIT|\"GET /robots.txt HTTP/1.1\"|100.2\r\n101|200|MISS|\"POST /myImages HTTP/1.1\"|319.4\r\n199|404|MISS|\"GET /not-found HTTP/1.1\"|142.9\r\n312|200|INVALIDATE|\"GET /robots.txt HTTP/1.1\"|245.1";
    }
}
