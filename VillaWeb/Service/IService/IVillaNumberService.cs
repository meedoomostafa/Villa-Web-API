using VillaWeb.Models.DTOs.VillaNumberDTOs;
using VillaWeb.Models.ResponseTypes;

namespace VillaWeb.Service.IService;

public interface IVillaNumberService
{
    Task<T> GetAllAsync<T>(string token) where T : APIResponse, new();
    Task<T> GetAsync<T>(int id,string token)  where T : APIResponse, new();
    Task<T> CreateAsync<T>(VillaNumberCreateDTO dto,string token)  where T : APIResponse, new();
    Task<T> UpdateAsync<T>(VillaNumberUpdateDTO dto,string token)   where T : APIResponse, new();
    Task<T> DeleteAsync<T>(int id,string token) where T : APIResponse, new();
}
