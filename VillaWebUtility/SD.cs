namespace VillaWebUtility;

public static class SD
{
    public enum ApiType
    {
        GET,
        POST,
        PUT,
        DELETE
    }
    public static string VillaApiBase = "/api/VillaApi";
    public static string VillaApiNumberBase = "/api/VillaNumberApi";
    public static string BookingApiBase = "/api/Booking";
    public static string VillaApiAuthenticationBase = "/api/Account";
    
    public static string GetVillaWithVillaNumbersEndPoint = "GetVillaWithVillaNumbers";
    
    public static string AccessTokenKey = "access_token";
    public static string RefreshTokenKey = "refresh_token";
    
    public enum BookingStatus
    {
        Pending,
        Confirmed,
        Cancelled
    }
}
