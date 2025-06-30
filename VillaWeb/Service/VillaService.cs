using VillaWeb.Models;
using VillaWeb.Models.DTOs.VillaDTOs;
using VillaWeb.Service.IService;
using VillaWebUtility;

namespace VillaWeb.Service;

public class VillaService : BaseService , IVillaService
{
    private readonly IHttpClientFactory _httpClient;
    private string _villaUrl;

    public VillaService(IHttpClientFactory httpClient,IConfiguration configuration) : base(httpClient)
    {
        _httpClient = httpClient;
        _villaUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI")!;
    }
    
    public Task<T> GetAllAsync<T>() where T : APIResponse, new()
    {
        return SendAsync<T>(new APIRequest
        {
            ApiType = SD.ApiType.GET, Url = $"{_villaUrl}{SD.VillaApiBase}", Data = null
        });
    }

    public Task<T> GetAsync<T>(int id) where T : APIResponse, new()
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.GET, Url = $"{_villaUrl}{SD.VillaApiBase}/{id}", Data = null
        });
    }

    public Task<T> CreateAsync<T>(VillaCreateDTO dto) where T : APIResponse, new()
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.POST, Url = $"{_villaUrl}{SD.VillaApiBase}", Data = dto
        });
    }

    public Task<T> UpdateAsync<T>(VillaUpdateDTO dto)  where T : APIResponse, new()
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.PUT, Url = $"{_villaUrl}{SD.VillaApiBase}/{dto.Id}", Data = dto
        });
    }

    public Task<T> DeleteAsync<T>(int id)  where T : APIResponse, new()
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.DELETE, Url = $"{_villaUrl}{SD.VillaApiBase}/{id}", Data = null
        });
    }
}
