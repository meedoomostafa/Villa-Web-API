using VillaWeb.Models.DTOs.AuthenticationDTOs;
using VillaWeb.Models.Requests;
using VillaWeb.Models.ResponseTypes;
using VillaWeb.Service.IService;
using VillaWebUtility;

namespace VillaWeb.Service;

public class AuthenticationService : BaseService, IAuthenticationService
{
    IHttpClientFactory _httpClientFactory;
    private readonly string _baseUrl;
    public AuthenticationService(IHttpClientFactory httpClient , IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(httpClient, httpContextAccessor,configuration)
    {
        _httpClientFactory = httpClient;
        _baseUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI")!;
    }

    public async Task<T> LoginAsync<T>(LoginDTO dto) where T : APIResponse, new()
    {
        return await SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.POST, Data = dto, Url = $"{_baseUrl}{SD.VillaApiAuthenticationBase}/Login"
        });
    }

    public async Task<T> RegisterAsync<T>(RegisterDTO dto) where T : APIResponse, new()
    {
        return await SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.POST, Data = dto, Url = $"{_baseUrl}{SD.VillaApiAuthenticationBase}/Register"
        });
    }
}