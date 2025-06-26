using VillaWeb.Models.DTOs.AuthenticationDTOs;
using VillaWeb.Models.ResponseTypes;

namespace VillaWeb.Service.IService;

public interface IAuthenticationService
{
    Task<T> LoginAsync<T>(LoginDTO dto) where T : APIResponse,new();
    Task<T> RegisterAsync<T>(RegisterDTO dto) where T : APIResponse,new();
}