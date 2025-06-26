using VillaWeb.Models.DTOs.VillaNumberDTOs;
using VillaWeb.Models.ResponseTypes;

namespace VillaWeb.Service.IService;

public interface IVillaNumberService
{
    Task<T> GetAllAsync<T>() where T : APIResponse, new();
    Task<T> GetAsync<T>(int id)  where T : APIResponse, new();
    Task<T> CreateAsync<T>(VillaNumberCreateDTO dto)  where T : APIResponse, new();
    Task<T> UpdateAsync<T>(VillaNumberUpdateDTO dto)   where T : APIResponse, new();
    Task<T> DeleteAsync<T>(int id) where T : APIResponse, new();
}
