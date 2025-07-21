using VillaWeb.Models.DTOs.VillaNumberDTOs;
using VillaWeb.Models.Requests;
using VillaWeb.Models.ResponseTypes;
using VillaWeb.Service.IService;
using VillaWebUtility;

namespace VillaWeb.Service;

public class VillaNumberService : BaseService, IVillaNumberService
{
    private readonly IHttpClientFactory _httpClient;
    private string _villaUrl;
    public VillaNumberService(IHttpClientFactory httpClient , IConfiguration configuration , IHttpContextAccessor httpContextAccessor) : base(httpClient, httpContextAccessor)
    {
        _httpClient = httpClient;
        _villaUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI")!;
    }

    public Task<T> GetAllAsync<T>(string token) where T : APIResponse, new()
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.GET, Url = $"{_villaUrl}{SD.VillaApiNumberBase}",Data = null , Token = token
        });
    }

    public Task<T> GetAsync<T>(int id,string token) where T : APIResponse, new()
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.GET, Url = $"{_villaUrl}{SD.VillaApiNumberBase}/{id}", Data = null, Token = token
        });
    }

    public Task<T> CreateAsync<T>(VillaNumberCreateDTO dto,string token) where T : APIResponse, new()
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.POST, Url = $"{_villaUrl}{SD.VillaApiNumberBase}", Data = dto, Token = token
        });
    }

    public Task<T> UpdateAsync<T>(VillaNumberUpdateDTO dto,string token) where T : APIResponse, new()
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.PUT, Url = $"{_villaUrl}{SD.VillaApiNumberBase}/{dto.VillaNo}", Data = dto,
            Token = token
        });
    }

    public Task<T> DeleteAsync<T>(int id,string token) where T : APIResponse, new()
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.DELETE, Url = $"{_villaUrl}{SD.VillaApiNumberBase}/{id}", Data = null, Token = token
        });
    }
}
