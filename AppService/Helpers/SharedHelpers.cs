using System.Net;
using VillaModels.Models;

namespace AppService.Helpers;

public static class SharedHelpers
{
    public static void ResetResponse(this APIResponse _response)
    {
        _response.IsSuccess = true;
        _response.StatusCode = HttpStatusCode.OK;
        _response.Result = null;
        _response.ErrorMessages.Clear();
    }
}