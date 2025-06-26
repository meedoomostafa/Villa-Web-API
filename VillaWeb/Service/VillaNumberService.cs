using VillaWeb.Models.DTOs.VillaNumberDTOs;
using VillaWeb.Models.Requests;
using VillaWeb.Models.ResponseTypes;
using VillaWeb.Service.IService;
using VillaWebUtility;

namespace VillaWeb.Service;

public class VillaNumberService : BaseService, IVillaNumberService
{
    private string _baseUrl;
    public VillaNumberService(IHttpClientFactory httpClient , IConfiguration configuration , IHttpContextAccessor httpContextAccessor) : base(httpClient, httpContextAccessor,configuration)
    {
        _baseUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI")!;
    }

    public async Task<T> GetAllAsync<T>() where T : APIResponse, new()
    {
        return await SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.GET, Url = $"{_baseUrl}{SD.VillaApiNumberBase}",Data = null
        });
    }

    public async Task<T> GetAsync<T>(int id) where T : APIResponse, new()
    {
        return await SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.GET, Url = $"{_baseUrl}{SD.VillaApiNumberBase}/{id}", Data = null
        });
    }

    public async Task<T> CreateAsync<T>(VillaNumberCreateDTO dto) where T : APIResponse, new()
    {
        return await SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.POST, Url = $"{_baseUrl}{SD.VillaApiNumberBase}", Data = dto
        });
    }

    public async Task<T> UpdateAsync<T>(VillaNumberUpdateDTO dto) where T : APIResponse, new()
    {
        return await SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.PUT, Url = $"{_baseUrl}{SD.VillaApiNumberBase}/{dto.VillaNo}", Data = dto
        });
    }

    public async Task<T> DeleteAsync<T>(int id) where T : APIResponse, new()
    {
        return await SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.DELETE, Url = $"{_baseUrl}{SD.VillaApiNumberBase}/{id}", Data = null
        });
    }
}
