namespace VillaModels.ResponseTypes;

public class LoginResponse //The default to put it in the result of APIResponse to unified the response form
{
    public string AccessToken { get; set; }        
    public DateTime AccessTokenExpiration { get; set; } 

    public string RefreshToken { get; set; }          
    public DateTime RefreshTokenExpiration { get; set; }

    public string DeviceId { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Role { get; set; } = null!;

}
