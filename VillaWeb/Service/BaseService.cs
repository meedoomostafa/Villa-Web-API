using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using VillaWeb.Models.Requests;
using VillaWeb.Models.ResponseTypes;
using VillaWeb.Service.IService;
using VillaWebUtility;

namespace VillaWeb.Service;

public class BaseService : IBaseService
{
    public APIResponse responseModel { get; set; }
    public IHttpClientFactory _httpClient { get; set; }
    public IHttpContextAccessor _httpContextAccessor { get; set; }
    
    private string _baseUrl;

    public BaseService(IHttpClientFactory httpClient, IHttpContextAccessor httpContextAccessor,IConfiguration configuration)
    {
        this.responseModel = new APIResponse();
        this._httpClient = httpClient;
        this._httpContextAccessor = httpContextAccessor;
        _baseUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI")!;;
    }

    public async Task<T> SendAsync<T>(APIRequest apiRequest) where T : APIResponse, new()
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
            
            var token = _httpContextAccessor.HttpContext!.Request.Cookies[SD.AccessTokenKey];

            if (!string.IsNullOrWhiteSpace(token))
            {
                client.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", token);
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

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var newToken = await TryRefreshTokenAsync();
                if (!string.IsNullOrEmpty(newToken))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", newToken);
                    return await SendAsync<T>(apiRequest);
                }
            }

            if (!response.IsSuccessStatusCode)
            {
                try
                {
                    var deserialized = JsonConvert.DeserializeObject<T>(content);
                    if (deserialized != null)
                    {
                        deserialized.StatusCode = response.StatusCode;
                        deserialized.IsSuccess = false;
                        return deserialized;
                    }
                }
                catch (JsonException)
                {
                    
                }

                return new T {
                    IsSuccess = false,
                    StatusCode = response.StatusCode,
                    ErrorMessages = new List<string> { "Unexpected error: " + content }
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
    
    private async Task<string?> TryRefreshTokenAsync()
    {
        var refreshToken = _httpContextAccessor.HttpContext!.Request.Cookies[SD.RefreshTokenKey];
        if (string.IsNullOrEmpty(refreshToken)) return null;

        using var client = _httpClient.CreateClient(nameof(IVillaService));
        var request = new HttpRequestMessage(HttpMethod.Post, $"{_baseUrl}{SD.VillaApiAuthenticationBase}/RefreshToken")
        {
            Content = new StringContent(
                JsonConvert.SerializeObject(new { RefreshToken = refreshToken }),
                Encoding.UTF8, "application/json")
        };

        var response = await client.SendAsync(request);
        if (!response.IsSuccessStatusCode) return null;

        var tokenResponse = JsonConvert.DeserializeObject<LoginResponse>(await response.Content.ReadAsStringAsync());
        if (tokenResponse == null) return null;

        var ctx = _httpContextAccessor.HttpContext!;
        ctx.Response.Cookies.Append(SD.AccessTokenKey, tokenResponse.AccessToken, new CookieOptions { /* … */ });
        ctx.Response.Cookies.Append(SD.RefreshTokenKey, tokenResponse.RefreshToken, new CookieOptions { /* … */ });

        return tokenResponse.AccessToken;
    }
}

