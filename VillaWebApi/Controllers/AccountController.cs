using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using VillaModels.Models;
using VillaModels.Models.DTOs.AuthenticationDTOs;
using VillaModels.RequestsTypes;
using VillaModels.ResponseTypes;
using VillaRepository.Repository.Interfaces;
using VillaWebApiUtilities;

namespace VillaWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    private readonly APIResponse _response;
    private readonly IUnitOfWork _unitOfWork;

    public AccountController(
        UserManager<ApplicationUser> userManager, 
        RoleManager<ApplicationRole> roleManager,
        IConfiguration configuration,
        IMapper mapper,
        IUnitOfWork unitOfWork
        )
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        this._response = new APIResponse() { ErrorMessages = new List<string>() };
    }

    #region Helpers
    private void ResetResponse()
    {
        _response.IsSuccess = true;
        _response.StatusCode = HttpStatusCode.OK;
        _response.Result = null;
        _response.ErrorMessages.Clear();
    }

    private static (string hash, string salt) CreateTokenHashAndSalt(string token)
    {
        var saltBytes = RandomNumberGenerator.GetBytes(16);
        var salt = Convert.ToBase64String(saltBytes);

        using var hmac = new HMACSHA256(saltBytes);
        var hash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(token)));

        return (hash, salt);
    }

    private static bool VerifyTokenWithSalt(string token, string storedHash, string storedSalt)
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

    private static string GeneratePlainRefreshToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64); // 512 bits
        return Convert.ToBase64String(bytes);
    }

    private (string token, JwtSecurityToken jwtToken) GenerateJwtAccessToken(ApplicationUser user, IList<string> roles)
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

    private void AddIdentityErrors(IdentityResult result)
    {
        _response.ErrorMessages.AddRange(result.Errors.Select(e => e.Description));
    }
    #endregion

    [HttpPost("RegisterCompany")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIResponse>> RegisterCompany([FromBody] RegisterCompanyDTO companyDTO)
    {
        ResetResponse();
        try
        {
            if (companyDTO == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("CompanyDTO is null");
                return BadRequest(_response);
            }
            var emailExistence = await _userManager.FindByEmailAsync(companyDTO.Email);
            if (emailExistence != null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("Email or Name already exists");
                return BadRequest(_response);
            }
            
            var companyUser = _mapper.Map<ApplicationUser>(companyDTO);
            var companyUserCreationResult = await _userManager.CreateAsync(companyUser, companyDTO.Password);
            if (!companyUserCreationResult.Succeeded)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                AddIdentityErrors(companyUserCreationResult);
                return BadRequest(_response);
            }

            var companyRoleAssigningResult =
                await _userManager.AddToRoleAsync(companyUser, ApplicationRoles.CompanyRoleName);
            if (!companyRoleAssigningResult.Succeeded)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                AddIdentityErrors(companyRoleAssigningResult);
                return BadRequest(_response);
            }
            
            var company = _mapper.Map<Company>(companyDTO);
            company.ApplicationUserId = companyUser.Id;
            await _unitOfWork.Company.CreateAsync(company);
            await _unitOfWork.SaveChangesAsync();
            
            _response.StatusCode = HttpStatusCode.OK;
            _response.Result = $"User {companyUser.UserName} Registered Successfully.";
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.InternalServerError;
            _response.ErrorMessages.Add(ex.Message);
            return StatusCode((int)HttpStatusCode.InternalServerError, _response);
        }
    }

    [HttpPost("Register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIResponse>> Register([FromBody]RegisterCustomerDTO registerDTO)
    {
        ResetResponse();
        try
        {
            var userExists = await _userManager.FindByEmailAsync(registerDTO.Email);
            if (userExists != null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("This email is already registered!");
                return BadRequest(_response);
            }
            
            var user = _mapper.Map<ApplicationUser>(registerDTO);
            var userCreationResult = await _userManager.CreateAsync(user, registerDTO.Password);
            var customer = _mapper.Map<Customer>(registerDTO);
            if (!userCreationResult.Succeeded)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                AddIdentityErrors(userCreationResult);
                return BadRequest(_response);
            }
            var roleAssigningResult = await _userManager.AddToRoleAsync(user,ApplicationRoles.CustomerRoleName);
            if (!roleAssigningResult.Succeeded)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                AddIdentityErrors(roleAssigningResult);
                return BadRequest(_response);
            }
            customer.ApplicationUserId = user.Id;
            await _unitOfWork.Customer.CreateAsync(customer);
            await _unitOfWork.SaveChangesAsync();
            
            _response.StatusCode = HttpStatusCode.OK;
            _response.Result = $"User {user.UserName} Registered Successfully.";
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.InternalServerError;
            _response.ErrorMessages.Add(ex.Message);
            return StatusCode((int)HttpStatusCode.InternalServerError, _response);
        }
    }

    [HttpPost("Login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIResponse>> Login([FromBody] LoginDTO loginDTO)
    {
        ResetResponse();
        try
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDTO.Password))
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.Unauthorized;
                _response.ErrorMessages.Add("Invalid login attempt");
                return Unauthorized(_response);
            }

            var roles = await _userManager.GetRolesAsync(user);

            var deviceId = Guid.NewGuid().ToString();

            var (jwt, jwtToken) = GenerateJwtAccessToken(user, roles);

            var plainRefresh = GeneratePlainRefreshToken();
            var (hash, salt) = CreateTokenHashAndSalt(plainRefresh);

            var refreshEntity = new RefreshToken
            {
                DeviceId = deviceId,
                TokenHash = hash,
                TokenSalt = salt,
                Created = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration["JWT:RefreshTokenLifeTime"])),
                UserId = user.Id,
                JwtTokenId = jwtToken.Id
            };

            var existing =
                await _unitOfWork.RefreshTokens.GetAllAsync(t =>
                    t.UserId == user.Id && t.DeviceId == deviceId && !t.IsRevoked);
            if (existing != null && existing.Any())
            {
                foreach (var item in existing)
                {
                    item.IsRevoked = true;
                    item.RevokedAt = DateTime.UtcNow;
                    item.RevokedReason = "New login from same device";
                    await _unitOfWork.RefreshTokens.UpdateAsync(item);
                }
            }
            await _unitOfWork.RefreshTokens.CreateAsync(refreshEntity);
            await _unitOfWork.SaveChangesAsync();

            var loginResponse = new LoginResponse()
            {
                AccessToken = jwt,
                AccessTokenExpiration = jwtToken.ValidTo,
                RefreshToken = plainRefresh,
                RefreshTokenExpiration = refreshEntity.ExpiresAt,
                UserName = user.UserName!,
                Email = user.Email!,
                Role = roles.FirstOrDefault() ?? "",
                DeviceId = deviceId 
            };

            _response.StatusCode = HttpStatusCode.OK;
            _response.Result = loginResponse;
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.InternalServerError;
            _response.ErrorMessages.Add("An unexpected error occurred.");
            return StatusCode((int)HttpStatusCode.InternalServerError, _response);
        }
    }

    [HttpPost("LogoutThisDevice")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIResponse>> LogoutThisDevice([FromBody] RefreshRequest refreshRequest)
    {
        ResetResponse();
        try
        {
            var token = await _unitOfWork.RefreshTokens
                .GetAsync(t => t.DeviceId == refreshRequest.DeviceId && !t.IsRevoked);

            if (token == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.Unauthorized;
                _response.ErrorMessages.Add("Invalid refresh token or Device ID");
                return Unauthorized(_response);
            }

            token.IsRevoked = true;
            token.RevokedAt = DateTime.UtcNow;
            token.RevokedReason = "User logged out";
            await _unitOfWork.RefreshTokens.UpdateAsync(token);
            await _unitOfWork.SaveChangesAsync();
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.InternalServerError;
            _response.ErrorMessages.Add("An unexpected error occurred.");
        }
        return StatusCode((int)HttpStatusCode.InternalServerError, _response);
    }

    [HttpPost("LogoutAllDevices")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIResponse>> LogoutAllDevices([FromBody] RefreshRequest refreshRequest)
    {
        ResetResponse();
        try
        {
            var token = await _unitOfWork.RefreshTokens
                .GetAsync(t => t.DeviceId == refreshRequest.DeviceId && !t.IsRevoked);

            if (token == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.Unauthorized;
                _response.ErrorMessages.Add("Invalid refresh token or Device ID");
                return Unauthorized(_response);
            }

            var tokens = await _unitOfWork.RefreshTokens
                .GetAllAsync(t => t.UserId == token.UserId);

            if (tokens == null || !tokens.Any())
            {
                _response.StatusCode = HttpStatusCode.OK;
                _response.ErrorMessages.Add("No Active Devices found");
                return Ok(_response);
            }

            foreach (var t in tokens)
            {
                t.IsRevoked = true;
                t.RevokedAt = DateTime.UtcNow;
                t.RevokedReason = "User logged out for all active devices";
                await _unitOfWork.RefreshTokens.UpdateAsync(t);
            }

            await _unitOfWork.SaveChangesAsync();
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.InternalServerError;
            _response.ErrorMessages.Add("An unexpected error occurred.");
        }
        return StatusCode((int)HttpStatusCode.InternalServerError, _response);
    }
    
    [HttpPost("RefreshToken")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequest model)
    {
        ResetResponse();
        try
        {
            if (string.IsNullOrWhiteSpace(model.DeviceId) || string.IsNullOrWhiteSpace(model.RefreshToken))
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("Device ID and refresh token are required");
                return Unauthorized(_response);
            }

            var storedTokens = await _unitOfWork.RefreshTokens
                .GetAllAsync(t => t.DeviceId == model.DeviceId && !t.IsRevoked);

            if (storedTokens == null || !storedTokens.Any())
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.Unauthorized;
                _response.ErrorMessages.Add("Invalid refresh token");
                return Unauthorized(_response);
            }
            
            RefreshToken validToken = null;
            foreach (var token in storedTokens)
            {
                if (VerifyTokenWithSalt(model.RefreshToken, token.TokenHash, token.TokenSalt))
                {
                    validToken = token;
                    break;
                }
            }
            
            if (validToken == null)
            {
                try
                {
                    var firstToken = storedTokens.First();
                    var allUserTokens = await _unitOfWork.RefreshTokens
                        .GetAllAsync(t => t.UserId == firstToken.UserId && !t.IsRevoked);
                
                    foreach (var t in allUserTokens)
                    {
                        t.IsRevoked = true;
                        t.RevokedAt = DateTime.UtcNow;
                        t.RevokedReason = "Refresh token reuse detected";
                        await _unitOfWork.RefreshTokens.UpdateAsync(t);
                    }
                    await _unitOfWork.SaveChangesAsync();
                }
                catch (Exception)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.InternalServerError;
                    _response.ErrorMessages.Add("Invalid refresh token");
                    return StatusCode((int)HttpStatusCode.InternalServerError, _response);
                }
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.Unauthorized;
                _response.ErrorMessages.Add("Invalid refresh token");
                return Unauthorized(_response);
            }

            if (validToken.IsRevoked || validToken.ExpiresAt < DateTime.UtcNow)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.Unauthorized;
                _response.ErrorMessages.Add("Invalid refresh token");
                return Unauthorized(_response);
            }
            
            var newPlain = GeneratePlainRefreshToken();
            var (newHash, newSalt) = CreateTokenHashAndSalt(newPlain);
            
            var user = await _userManager.FindByIdAsync(validToken.UserId.ToString());
            if (user == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.Unauthorized;
                _response.ErrorMessages.Add("User not found");
                return Unauthorized(_response);
            }

            var roles = await _userManager.GetRolesAsync(user);
            var (newJwt, newJwtToken) = GenerateJwtAccessToken(user, roles);

            validToken.TokenHash = newHash;
            validToken.TokenSalt = newSalt;
            validToken.Created = DateTime.UtcNow;
            validToken.ExpiresAt = DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration["JWT:RefreshTokenLifeTime"]!));
            validToken.JwtTokenId = newJwtToken.Id;
            validToken.IsRevoked = false; 
            validToken.RevokedAt = null;
            validToken.RevokedReason = null;

            await _unitOfWork.RefreshTokens.UpdateAsync(validToken);
            await _unitOfWork.SaveChangesAsync();

            var loginResponse = new LoginResponse()
            {
                AccessToken = newJwt,
                AccessTokenExpiration = newJwtToken.ValidTo,
                RefreshToken = newPlain, 
                RefreshTokenExpiration = validToken.ExpiresAt,
                UserName = user.UserName!,
                Email = user.Email!,
                Role = roles.FirstOrDefault() ?? "",
                DeviceId = validToken.DeviceId
            };

            _response.StatusCode = HttpStatusCode.OK;
            _response.Result = loginResponse;
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.InternalServerError;
            _response.ErrorMessages.Add("An unexpected error occurred.");
        }
        return StatusCode((int)HttpStatusCode.InternalServerError, _response);
    }
}
