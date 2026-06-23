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
        _httpClient.BaseAddress = new Uri("https://localhost:7200/api/");
    }

    // --- КЛИЕНТЫ (GET) ---
    public async Task<List<ClientModel>> GetClientsAsync()
    {
        try
        {
            var clients = await _httpClient.GetFromJsonAsync<List<ClientModel>>("Clients");
            return clients ?? new List<ClientModel>();
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Ошибка API (Clients GET): {ex.Message}");
            return new List<ClientModel>();
        }
    }

    // --- КЛИЕНТЫ (POST) ---
    public async Task<bool> CreateClientAsync(ClientModel client)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("Clients", client);
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Ошибка API (Clients POST): {ex.Message}");
            return false;
        }
    }

    // --- КЛИЕНТЫ (PUT) ---
    public async Task<bool> UpdateClientAsync(int id, ClientModel client)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"Clients/{id}", client);
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Ошибка API (Clients PUT): {ex.Message}");
            return false;
        }
    }

    // --- КЛИЕНТЫ (DELETE) 👇 НОВЫЙ МЕТОД ---
    public async Task<bool> DeleteClientAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"Clients/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Ошибка API (Clients DELETE): {ex.Message}");
            return false;
        }
    }

    // --- ТРЕНЕРЫ ---
    public async Task<List<Trainer>> GetTrainersAsync()
    {
        try
        {
            var trainers = await _httpClient.GetFromJsonAsync<List<Trainer>>("Trainers");
            return trainers ?? new List<Trainer>();
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Ошибка API (Trainers): {ex.Message}");
            return new List<Trainer>();
        }
    }

    // --- АБОНЕМЕНТЫ ---
    public async Task<List<Subscription>> GetSubscriptionsAsync()
    {
        try
        {
            var subs = await _httpClient.GetFromJsonAsync<List<Subscription>>("Subscriptions");
            return subs ?? new List<Subscription>();
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Ошибка API (Subscriptions): {ex.Message}");
            return new List<Subscription>();
        }
    }

    // --- ТРЕНИРОВКИ ---
    public async Task<List<TrainingSession>> GetTrainingSessionsAsync()
    {
        try
        {
            var sessions = await _httpClient.GetFromJsonAsync<List<TrainingSession>>("TrainingSessions");
            return sessions ?? new List<TrainingSession>();
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Ошибка API (TrainingSessions): {ex.Message}");
            return new List<TrainingSession>();
        }
    }
}