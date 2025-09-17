using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using AppModels.Models;

namespace AppService.Helpers;

public class AccountHelper
{
    private readonly IConfiguration _configuration;
    private APIResponse _response;

    public AccountHelper(IConfiguration configuration)
    {
        _configuration = configuration;
        _response = new APIResponse()
        {
            ErrorMessages = new List<string>()
        };
    }

    public (string hash, string salt) CreateTokenHashAndSalt(string token)
    {
        var saltBytes = RandomNumberGenerator.GetBytes(16);
        var salt = Convert.ToBase64String(saltBytes);

        using var hmac = new HMACSHA256(saltBytes);
        var hash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(token)));

        return (hash, salt);
    }

    public bool VerifyTokenWithSalt(string token, string storedHash, string storedSalt)
    {
        try
        {
            var saltBytes = Convert.FromBase64String(storedSalt);
            using var hmac = new HMACSHA256(saltBytes);
            var computed = hmac.ComputeHash(Encoding.UTF8.GetBytes(token));
            var storedHashBytes = Convert.FromBase64String(storedHash);

            return CryptographicOperations
                .FixedTimeEquals(computed, storedHashBytes); 
        }
        catch
        {
            return false;
        }
    } 

    public string GeneratePlainRefreshToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64); // 512 bits
        return Convert.ToBase64String(bytes);
    }

    public (string token, JwtSecurityToken jwtToken) GenerateJwtAccessToken(ApplicationUser user, IList<string> roles)
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!)
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var jwtToken = new JwtSecurityToken(
            issuer: _configuration["JWT:Issuer"],
            audience: _configuration["JWT:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JWT:LifeTime"]!)),
            signingCredentials: creds
        );

        var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        return (token, jwtToken);
    }

    public void AddIdentityErrors(IdentityResult result)
    {
        _response.ErrorMessages.AddRange(result.Errors.Select(e => e.Description));
    }
}