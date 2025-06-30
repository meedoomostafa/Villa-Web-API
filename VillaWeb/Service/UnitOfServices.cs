using VillaWeb.Service.IService;

namespace VillaWeb.Service;

public class UnitOfServices : IUnitOfServices
{
    private readonly IHttpClientFactory _httpClient;
    private readonly IConfiguration _configuration;
    
    public IVillaService VillaService { get; private set; }
    public IVillaNumberService VillaNumberService { get; private set; }

    
    public UnitOfServices(IHttpClientFactory httpClient , IConfiguration configuration)
    {
        _httpClient = httpClient;
        VillaService = new VillaService(httpClient,configuration);
        VillaNumberService = new VillaNumberService(httpClient,configuration);
    }
}
