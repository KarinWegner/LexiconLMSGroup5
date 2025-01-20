using LMS.Blazor.Client.Models;
using LMS.Shared.DTOs;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace LMS.Blazor.Client.Services;

public class ClientApiService(IHttpClientFactory httpClientFactory, NavigationManager navigationManager) : IApiService
{
    private readonly HttpClient httpClient = httpClientFactory.CreateClient("BffClient");

    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };


    public async Task<TResponse?> GetAsync<TResponse>(string endpoint)
    {
        return await CallApiAsync<object?, TResponse>(
            endpoint,
            HttpMethod.Get,
            null
        );
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(
        string endpoint,
        TRequest dto)
    {
        return await CallApiAsync<TRequest, TResponse>(
            endpoint,
            HttpMethod.Post,
            dto
        );
    }

    private async Task<TResponse?> CallApiAsync<TRequest, TResponse>(string endpoint, HttpMethod httpMethod, TRequest? dto)
    {
        var request = new HttpRequestMessage(httpMethod, $"proxy-endpoint/{endpoint}");
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        if (httpMethod != HttpMethod.Get && dto is not null)
        {
            var serialized = JsonSerializer.Serialize(dto);
            request.Content = new StringContent(serialized);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        }

        var response = await httpClient.SendAsync(request);

        if (response.StatusCode == System.Net.HttpStatusCode.Forbidden
           || response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            navigationManager.NavigateTo("AccessDenied");
        }

        response.EnsureSuccessStatusCode();

        var res = await JsonSerializer.DeserializeAsync<TResponse>(await response.Content.ReadAsStreamAsync(), _jsonSerializerOptions, CancellationToken.None);
        return res;
    }

    public async Task<TResponse?> CallRegisterAsync<TRequest, TResponse>(string endpoint, TRequest? dto)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"https://localhost:7044/{endpoint}");
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


        var serialized = JsonSerializer.Serialize(dto);
        request.Content = new StringContent(serialized);
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        

        var response = await httpClient.SendAsync(request);

        

        response.EnsureSuccessStatusCode();

        if (!response.IsSuccessStatusCode)
        {
            // Handle non-successful status codes
            var errorResponse = await response.Content.ReadAsStringAsync();
            var errors = new List<string> { $"Error: {response.StatusCode}, Message: {errorResponse}" };
            if (typeof(TResponse) == typeof(ApiResponse))
            {
                return (TResponse)(object)new ApiResponse { Success = false, Errors = errors };
            }
            return default;
        }

        if (response.Content.Headers.ContentLength.GetValueOrDefault() == 0)
        {
            // Return a successful ApiResponse if the response is empty
            if (typeof(TResponse) == typeof(ApiResponse))
            {
                return (TResponse)(object)new ApiResponse { Success = true };
            }
            return default;
        }

        // return new ApiResponse();
        var res = await JsonSerializer.DeserializeAsync<TResponse>(await response.Content.ReadAsStreamAsync(), _jsonSerializerOptions, CancellationToken.None);
        return res;

    }
}