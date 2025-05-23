using System.Net.Http.Json;

namespace AccessControlSystem.Infrastructure.Http.Clients.Airfob;

public class AirfobClient(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<T> GetAsync<T>(string endpoint)
    {
        var response = await _httpClient.GetAsync(endpoint);

        response.EnsureSuccessStatusCode();

        return (await response.Content.ReadFromJsonAsync<T>())!;
    }

    public async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(endpoint, request);

        response.EnsureSuccessStatusCode();

        return (await response.Content.ReadFromJsonAsync<TResponse>())!;
    }
}
