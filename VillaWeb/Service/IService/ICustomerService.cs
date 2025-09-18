using VillaWeb.Models.DTOs.ProfilesDTOs;
using VillaWeb.Models.ResponseTypes;

namespace VillaWeb.Service.IService;

public interface ICustomerService
{
    Task<T> GetCustomerAsync<T>(int userId) where T : APIResponse, new();
    Task<T> UpdateCustomerAsync<T>(CustomerProfileDTO dto) where T : APIResponse, new();
}
