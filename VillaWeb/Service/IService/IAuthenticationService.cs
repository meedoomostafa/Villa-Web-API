using VillaWeb.Models.DTOs.AuthenticationDTOs;
using VillaWeb.Models.ResponseTypes;

namespace VillaWeb.Service.IService;

public interface IAuthenticationService
{
    Task<T> LoginAsync<T>(LoginDTO dto) where T : APIResponse,new();
    Task<T> RegisterCustomerAsync<T>(RegisterCustomerDTO customerDto) where T : APIResponse,new();
    Task<T> RegisterCompanyAsync<T>(RegisterCompanyDTO companyDto) where T : APIResponse,new();
}