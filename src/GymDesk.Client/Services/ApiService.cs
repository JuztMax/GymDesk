using System.Net.Http;
using System.Net.Http.Json;
using GymDesk.Client.Models;
using ClientModel = GymDesk.Client.Models.Client;

namespace GymDesk.Client.Services;
public class ApiService
{
    private readonly HttpClient _httpClient;

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        // Порт должен совпадать с тем, что в launchSettings.json твоего API
        _httpClient.BaseAddress = new Uri("https://localhost:7200/api/");
    }

    public async Task<List<ClientModel>> GetClientsAsync()
    {
        try
        {
            var clients = await _httpClient.GetFromJsonAsync<List<ClientModel>>("Clients");
            return clients ?? new List<ClientModel>();
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Ошибка API: {ex.Message}");
            return new List<ClientModel>();
        }
    }
}