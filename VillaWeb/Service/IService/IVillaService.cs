using VillaWeb.Models;
using VillaWeb.Models.DTOs.VillaDTOs;

namespace VillaWeb.Service.IService;

public interface IVillaService
{
    Task<T> GetAllAsync<T>() where T : APIResponse, new();
    Task<T> GetAsync<T>(int id)  where T : APIResponse, new();
    Task<T> CreateAsync<T>(VillaCreateDTO dto)  where T : APIResponse, new();
    Task<T> UpdateAsync<T>(VillaUpdateDTO dto)   where T : APIResponse, new();
    Task<T> DeleteAsync<T>(int id) where T : APIResponse, new();
}
