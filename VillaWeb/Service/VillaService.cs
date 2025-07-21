using VillaWeb.Models.DTOs.VillaDTOs;
using VillaWeb.Models.Requests;
using VillaWeb.Models.ResponseTypes;
using VillaWeb.Service.IService;
using VillaWebUtility;

namespace VillaWeb.Service;

public class VillaService : BaseService , IVillaService
{
    private readonly IHttpClientFactory _httpClient;
    private string _villaUrl;

    public VillaService(IHttpClientFactory httpClient,IConfiguration configuration , IHttpContextAccessor httpContextAccessor) : base(httpClient,httpContextAccessor)
    {
        _httpClient = httpClient;
        _villaUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI")!;
    }
    
    public Task<T> GetAllAsync<T>(string token) where T : APIResponse, new()
    {
        return SendAsync<T>(new APIRequest
        {
            ApiType = SD.ApiType.GET, Url = $"{_villaUrl}{SD.VillaApiBase}", Data = null , Token = token
        });

    }

    public Task<T> GetAsync<T>(int id,string token) where T : APIResponse, new()
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.GET, Url = $"{_villaUrl}{SD.VillaApiBase}/{id}", Data = null , Token = token
        });
    }

    public Task<T> CreateAsync<T>(VillaCreateDTO dto,string token) where T : APIResponse, new()
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.POST, Url = $"{_villaUrl}{SD.VillaApiBase}", Data = dto , Token = token
        });
    }

    public Task<T> UpdateAsync<T>(VillaUpdateDTO dto,string token)  where T : APIResponse, new()
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.PUT, Url = $"{_villaUrl}{SD.VillaApiBase}/{dto.Id}", Data = dto , Token = token
        });
    }

    public Task<T> DeleteAsync<T>(int id,string token)  where T : APIResponse, new()
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.DELETE, Url = $"{_villaUrl}{SD.VillaApiBase}/{id}", Data = null , Token = token
        });
    }
}
