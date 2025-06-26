using VillaWeb.Models.DTOs.VillaDTOs;
using VillaWeb.Models.Requests;
using VillaWeb.Models.ResponseTypes;
using VillaWeb.Service.IService;
using VillaWebUtility;

namespace VillaWeb.Service;

public class VillaService : BaseService , IVillaService
{
    private string _baseUrl;

    public VillaService(IHttpClientFactory httpClient,IConfiguration configuration , IHttpContextAccessor httpContextAccessor) : base(httpClient,httpContextAccessor,configuration)
    {
        _baseUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI")!;
    }
    
    public Task<T> GetAllAsync<T>() where T : APIResponse, new()
    {
        return SendAsync<T>(new APIRequest
        {
            ApiType = SD.ApiType.GET, Url = $"{_baseUrl}{SD.VillaApiBase}", Data = null 
        });

    }

    public Task<T> GetVillaWithVillaNumberAsync<T>(int id) where T : APIResponse, new()
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.GET, Url = $"{_baseUrl}{SD.VillaApiBase}/{SD.GetVillaWithVillaNumbersEndPoint}/{id}",
            Data = null
        });
    }

    public Task<T> GetAsync<T>(int id) where T : APIResponse, new()
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.GET, Url = $"{_baseUrl}{SD.VillaApiBase}/{id}", Data = null
        });
    }

    public Task<T> CreateAsync<T>(VillaCreateDTO dto) where T : APIResponse, new()
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.POST, Url = $"{_baseUrl}{SD.VillaApiBase}", Data = dto 
        });
    }

    public Task<T> UpdateAsync<T>(VillaUpdateDTO dto)  where T : APIResponse, new()
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.PUT, Url = $"{_baseUrl}{SD.VillaApiBase}/{dto.Id}", Data = dto
        });
    }

    public Task<T> DeleteAsync<T>(int id)  where T : APIResponse, new()
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.DELETE, Url = $"{_baseUrl}{SD.VillaApiBase}/{id}", Data = null
        });
    }
}
