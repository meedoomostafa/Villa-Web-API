using System.Net;
using System.Text;
using Newtonsoft.Json;
using VillaWeb.Models;
using VillaWeb.Service.IService;
using VillaWebUtility;

namespace VillaWeb.Service;

public class BaseService : IBaseService
{
    public APIResponse responseModel { get; set; }
    public IHttpClientFactory _httpClient { get; set; }

    public BaseService(IHttpClientFactory httpClient)
    {
        this.responseModel = new APIResponse();
        this._httpClient = httpClient;
    }

    public async Task<T> SendAsync<T>(APIRequest apiRequest) where T : APIResponse , new()
    {
        try
        {
            if (apiRequest == null)
            {
                return new T { 
                    IsSuccess = false, 
                    ErrorMessages = new List<string> { "API Request is null" } 
                };
            }

            using var client = _httpClient.CreateClient(nameof(IVillaService));
            using var message = new HttpRequestMessage();
            
            message.Headers.Add("Accept", "application/json");
            message.RequestUri = new Uri(apiRequest.Url);
            
            if (apiRequest.Data != null)
            {
                message.Content = new StringContent(
                    JsonConvert.SerializeObject(apiRequest.Data),
                    Encoding.UTF8,
                    "application/json");
            }

            message.Method = apiRequest.ApiType switch
            {
                SD.ApiType.POST => HttpMethod.Post,
                SD.ApiType.PUT => HttpMethod.Put,
                SD.ApiType.DELETE => HttpMethod.Delete,
                _ => HttpMethod.Get
            };

            using var response = await client.SendAsync(message);
            var content = await response.Content.ReadAsStringAsync();

            // Handle non-success responses
            if (!response.IsSuccessStatusCode)
            {
                return new T {
                    IsSuccess = false,
                    StatusCode = response.StatusCode,
                    ErrorMessages = new List<string> { content }
                };
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                return new T
                {
                    IsSuccess = true, 
                    StatusCode = response.StatusCode
                };
            }

            var result = JsonConvert.DeserializeObject<T>(content) ?? new T {
                IsSuccess = false,
                ErrorMessages = new List<string> { "Failed to deserialize API response" }
            };
            
            result.StatusCode = response.StatusCode;
            return result;
        }
        catch (Exception ex)
        {
            return new T {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                ErrorMessages = new List<string> { ex.Message }
            };
        }
    }
}

