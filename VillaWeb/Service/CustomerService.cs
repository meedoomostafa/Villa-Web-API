using VillaWeb.Models.DTOs.ProfilesDTOs;
using VillaWeb.Models.Requests;
using VillaWeb.Models.ResponseTypes;
using VillaWeb.Service.IService;
using VillaWebUtility;

namespace VillaWeb.Service;

public class CustomerService : BaseService, ICustomerService
{
    private readonly string _baseUrl;

    public CustomerService(IHttpClientFactory httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) 
        : base(httpClient, httpContextAccessor, configuration)
    {
        _baseUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI")!;
    }

    public async Task<T> GetCustomerAsync<T>(int userId) where T : APIResponse, new()
    {
        return await SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.GET,
            Url = $"{_baseUrl}{SD.VillaApiCustomerBase}/{userId}"
        });
    }

    public async Task<T> UpdateCustomerAsync<T>(CustomerProfileDTO dto) where T : APIResponse, new()
    {
        return await SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.PUT,
            Data = dto,
            Url = $"{_baseUrl}{SD.VillaApiCustomerBase}"
        });
    }
}
