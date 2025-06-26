namespace VillaWebApi;

public class JwtOptions
{
    string Key { get; set; }
    string Issuer { get; set; }
    string Audience { get; set; }
    int LifeTime { get; set; }
}
