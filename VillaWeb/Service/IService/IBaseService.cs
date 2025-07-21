using VillaWeb.Models.Requests;
using VillaWeb.Models.ResponseTypes;

namespace VillaWeb.Service.IService;

public interface IBaseService
{
    APIResponse responseModel {get; set; }
    Task<T> SendAsync<T>(APIRequest request) where T : APIResponse, new();
}
