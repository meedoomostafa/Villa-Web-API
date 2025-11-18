using VillaWeb.Models.DTOs.ProfilesDTOs;
using VillaWeb.Models.Requests;
using VillaWeb.Models.ResponseTypes;
using VillaWeb.Service.IService;
using VillaWebUtility;

namespace VillaWeb.Service;

public class CompanyService : BaseService, ICompanyService
{
    private readonly string _baseUrl;

    public CompanyService(IHttpClientFactory httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) 
        : base(httpClient, httpContextAccessor, configuration)
    {
        _baseUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI")!;
    }

    public async Task<T> GetCompanyAsync<T>(int userId) where T : APIResponse, new()
    {
        return await SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.GET,
            Url = $"{_baseUrl}{SD.VillaApiCompanyBase}/{userId}/Profile"
        });
    }

    public async Task<T> UpdateCompanyAsync<T>(CompanyProfileDTO dto) where T : APIResponse, new()
    {
        return await SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.PUT,
            Data = dto,
            Url = $"{_baseUrl}{SD.VillaApiCompanyBase}"
        });
    }

    public async Task<T> GetDashboardAsync<T>(int companyId) where T : APIResponse, new()
    {
        return await SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.GET,
            Url = $"{_baseUrl}{SD.VillaApiCompanyBase}/{companyId}/Dashboard"
        });
    }
}
