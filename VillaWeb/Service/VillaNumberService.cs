using VillaWeb.Models;
using VillaWeb.Models.DTOs.VillaNumberDTOs;
using VillaWeb.Service.IService;
using VillaWebUtility;

namespace VillaWeb.Service;

public class VillaNumberService : BaseService, IVillaNumberService
{
    private readonly IHttpClientFactory _httpClient;
    private string _villaUrl;
    public VillaNumberService(IHttpClientFactory httpClient , IConfiguration configuration) : base(httpClient)
    {
        _httpClient = httpClient;
        _villaUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI")!;
    }

    public Task<T> GetAllAsync<T>() where T : APIResponse, new()
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.GET, Url = $"{_villaUrl}{SD.VillaApiNumberBase}",Data = null
        });
    }

    public Task<T> GetAsync<T>(int id) where T : APIResponse, new()
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.GET, Url = $"{_villaUrl}{SD.VillaApiNumberBase}/{id}", Data = null
        });
    }

    public Task<T> CreateAsync<T>(VillaNumberCreateDTO dto) where T : APIResponse, new()
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.POST, Url = $"{_villaUrl}{SD.VillaApiNumberBase}", Data = dto
        });
    }

    public Task<T> UpdateAsync<T>(VillaNumberUpdateDTO dto) where T : APIResponse, new()
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.PUT, Url = $"{_villaUrl}{SD.VillaApiNumberBase}/{dto.VillaNo}", Data = dto
        });
    }

    public Task<T> DeleteAsync<T>(int id) where T : APIResponse, new()
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.DELETE, Url = $"{_villaUrl}{SD.VillaApiNumberBase}/{id}", Data = null
        });
    }
}
