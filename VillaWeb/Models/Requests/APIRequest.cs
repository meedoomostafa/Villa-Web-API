using static VillaWebUtility.SD;

namespace VillaWeb.Models.Requests;

public class APIRequest
{
    public ApiType ApiType { get; set; } = ApiType.GET;
    public string Url { get; set; }
    public object Data { get; set; } //body(json)
    public string Token { get; set; }
}
