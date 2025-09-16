using System.Net;

namespace VillaModels.Models;

public class APIResponse
{
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
    public bool IsSuccess { get; set; } = true;
    public List<string> ErrorMessages { get; set; }
    public virtual object Result { get; set; }
}
