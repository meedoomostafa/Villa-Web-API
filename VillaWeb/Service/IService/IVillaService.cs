using VillaWeb.Models.DTOs.VillaDTOs;
using VillaWeb.Models.ResponseTypes;

namespace VillaWeb.Service.IService;

public interface IVillaService
{
    Task<T> GetAllAsync<T>(string token) where T : APIResponse, new();
    Task<T> GetAsync<T>(int id,string token)  where T : APIResponse, new();
    Task<T> CreateAsync<T>(VillaCreateDTO dto,string token)  where T : APIResponse, new();
    Task<T> UpdateAsync<T>(VillaUpdateDTO dto,string token)   where T : APIResponse, new();
    Task<T> DeleteAsync<T>(int id,string token) where T : APIResponse, new();
}
