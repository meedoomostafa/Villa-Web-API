using VillaWeb.Models;

namespace VillaWeb.Service.IService;

public interface IBaseService
{
    APIResponse responseModel {get; set; }
    Task<T> SendAsync<T>(APIRequest request) where T : APIResponse, new();
}
