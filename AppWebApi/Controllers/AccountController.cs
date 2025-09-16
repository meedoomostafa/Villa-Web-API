using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VillaModels.Models;
using VillaModels.Models.DTOs.AuthenticationDTOs;
using VillaModels.ResponseTypes;
using AppRepository.Repository.Interfaces;
using AppWebApiUtilities;
using AppService.Helpers;
using AppService.Interfaces;
using RefreshRequest = VillaModels.RequestsTypes.RefreshRequest;

namespace AppWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    private readonly APIResponse _response;
    private readonly IUnitOfServices _unitOfServices;
    private readonly IUnitOfWork _unitOfWork;
    private readonly AccountHelper _accountHelper;

    public AccountController(
        UserManager<ApplicationUser> userManager, 
        RoleManager<ApplicationRole> roleManager,
        IConfiguration configuration,
        IMapper mapper,
        IUnitOfServices unitOfServices,
        IUnitOfWork unitOfWork,
        AccountHelper accountHelper)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _mapper = mapper;
        _unitOfServices = unitOfServices;
        _unitOfWork = unitOfWork;
        _accountHelper = accountHelper;
        this._response = new APIResponse() { ErrorMessages = new List<string>() };
    }

    [HttpPost("RegisterCompany")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIResponse>> RegisterCompany([FromBody] RegisterCompanyDTO companyDTO)
    {
        _response.ResetResponse();
        try
        {
            if (companyDTO == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("CompanyDTO is null");
                return BadRequest(_response);
            }
            var emailExistence = await _unitOfServices.Account
                .GetUserByEmailAsync(companyDTO.Email);
            if (emailExistence != null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("Email or Name already exists");
                return BadRequest(_response);
            }
            
            var companyUser = _mapper.Map<ApplicationUser>(companyDTO);
            var companyUserCreationResult = await _unitOfServices.Account.CreateUserAsync(companyUser , companyDTO.Password);
            if (!companyUserCreationResult.Succeeded)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _accountHelper.AddIdentityErrors(companyUserCreationResult);
                return BadRequest(_response);
            }

            var companyRoleAssigningResult =
                await _unitOfServices.Account.AssignUserToRoleAsync(companyUser, ApplicationRoles.CompanyRoleName);
            if (!companyRoleAssigningResult.Succeeded)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _accountHelper.AddIdentityErrors(companyRoleAssigningResult);
                return BadRequest(_response);
            }
            
            var company = _mapper.Map<Company>(companyDTO);
            company.ApplicationUserId = companyUser.Id;
            await _unitOfServices.Companies.CreateCompanyUser(company);
            await _unitOfServices.SaveChangesAsync();
            
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
        _response.ResetResponse();
        try
        {
            var userExists = await _unitOfServices.Account
                .GetUserByEmailAsync(registerDTO.Email);
            if (userExists != null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("This email is already registered!");
                return BadRequest(_response);
            }
            
            var user = _mapper.Map<ApplicationUser>(registerDTO);
            var userCreationResult = await _unitOfServices.Account.CreateUserAsync(user, registerDTO.Password);
            var customer = _mapper.Map<Customer>(registerDTO);
            if (!userCreationResult.Succeeded)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _accountHelper.AddIdentityErrors(userCreationResult);
                return BadRequest(_response);
            }
            var roleAssigningResult = await _unitOfServices.Account.AssignUserToRoleAsync(user,ApplicationRoles.CustomerRoleName);
            if (!roleAssigningResult.Succeeded)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _accountHelper.AddIdentityErrors(roleAssigningResult);
                return BadRequest(_response);
            }
            customer.ApplicationUserId = user.Id;
            await _unitOfServices.Customers.CreateCustomerUser(customer);
            await _unitOfServices.SaveChangesAsync();
            
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
        _response.ResetResponse();
        try
        {
            var user = await _unitOfServices.Account.GetUserByEmailAsync(loginDTO.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDTO.Password))
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.Unauthorized;
                _response.ErrorMessages.Add("Invalid login attempt");
                return Unauthorized(_response);
            }

            var roles = await _unitOfServices.Account.GetRolesAsync(user);

            var deviceId = Guid.NewGuid().ToString();

            var (jwt, jwtToken) = _accountHelper.GenerateJwtAccessToken(user, roles);

            var plainRefresh = _accountHelper.GeneratePlainRefreshToken();
            var (hash, salt) = _accountHelper.CreateTokenHashAndSalt(plainRefresh);

            var refreshEntity = new RefreshToken
            {
                DeviceId = deviceId,
                TokenHash = hash,
                TokenSalt = salt,
                Created = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow
                    .AddDays(Convert.ToDouble(_configuration["JWT:RefreshTokenLifeTime"])),
                UserId = user.Id,
                JwtTokenId = jwtToken.Id
            };

            var existing =
                await _unitOfServices.RefreshTokens
                    .GetAllRefreshTokensForUserDeviceNotRevokedAsync(user.Id, deviceId);
            
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
            await _unitOfServices.RefreshTokens.CreateRefreshTokenAsync(refreshEntity);
            await _unitOfServices.SaveChangesAsync();

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
        _response.ResetResponse();
        try
        {
            var token = await _unitOfServices.RefreshTokens
                .GetRefreshTokenWithDeviceIdNotRevokedAsync(refreshRequest.DeviceId);

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
        _response.ResetResponse();
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
                await _unitOfServices.RefreshTokens.UpdateRefreshTokenAsync(t);
            }

            await _unitOfServices.SaveChangesAsync();
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

    // [HttpPost("ResetPassword")]
    // [ProducesResponseType(StatusCodes.Status200OK)]
    // [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    // [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    // public async Task<ActionResult<APIResponse>> ResetPassword([FromBody] ResetPasswordRequest ResetRequest)
    // {
    //     
    // }
    
    [HttpPost("RefreshToken")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequest model)
    {
        _response.ResetResponse();
        try
        {
            if (string.IsNullOrWhiteSpace(model.DeviceId) || string.IsNullOrWhiteSpace(model.RefreshToken))
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("Device ID and refresh token are required");
                return Unauthorized(_response);
            }

            var storedTokens = await _unitOfServices.RefreshTokens
                .GetAllRefreshTokensForDeviceIdNotRevokedAsync(model.DeviceId);
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
                if (_accountHelper.VerifyTokenWithSalt(model.RefreshToken, token.TokenHash, token.TokenSalt))
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
                    var allUserTokens = await _unitOfServices.RefreshTokens
                        .GetAllUserTokensByUserIdNotRevokedAsync(validToken!.UserId);
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
            
            var newPlain = _accountHelper.GeneratePlainRefreshToken();
            var (newHash, newSalt) = _accountHelper.CreateTokenHashAndSalt(newPlain);
            
            var user = await _userManager.FindByIdAsync(validToken.UserId.ToString());
            if (user == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.Unauthorized;
                _response.ErrorMessages.Add("User not found");
                return Unauthorized(_response);
            }

            var roles = await _userManager.GetRolesAsync(user);
            var (newJwt, newJwtToken) = _accountHelper.GenerateJwtAccessToken(user, roles);

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
