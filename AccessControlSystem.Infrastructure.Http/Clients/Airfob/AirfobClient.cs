using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Shared;
using System.Net.Http.Json;

namespace AccessControlSystem.Infrastructure.Http.Clients.Airfob;

public class AirfobClient(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<AirfobResponse<T>> GetAsync<T>(string endpoint) where T : class
    {
        try
        {
            var response = await _httpClient.GetAsync(endpoint);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<T>();

            return AirfobResponse<T>.CreateSuccessResponse(result!);
        }

        catch (Exception)
        {
            return AirfobResponse<T>.CreateFailResponse();
        }
    }

    public async Task<AirfobResponse<TResponse>> PostAsync<TRequest, TResponse>(
        string endpoint,
        TRequest request) where TResponse : class
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(endpoint, request);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<TResponse>();

            return AirfobResponse<TResponse>.CreateSuccessResponse(result!);
        }

        catch (Exception)
        {
            return AirfobResponse<TResponse>.CreateFailResponse();
        }
    }

    public async Task<AirfobResponse<TResponse>> PatchAsync<TRequest, TResponse>(
        string endpoint,
        TRequest request) where TResponse : class
    {
        try
        {
            var response = await _httpClient.PatchAsJsonAsync(endpoint, request);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<TResponse>();

            return AirfobResponse<TResponse>.CreateSuccessResponse(result!);
        }

        catch (Exception)
        {
            return AirfobResponse<TResponse>.CreateFailResponse();
        }
    }

    public async Task<AirfobResponse<TResponse>> DeleteAsync<TResponse>(
        string endpoint)
    {
        try
        {
            var response = await _httpClient.DeleteAsync(endpoint);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<TResponse>();

            return AirfobResponse<TResponse>.CreateSuccessResponse(result!);
        }

        catch (Exception)
        {
            return AirfobResponse<TResponse>.CreateFailResponse();
        }
    }
}