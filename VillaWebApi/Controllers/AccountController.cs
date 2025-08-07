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

    public AccountController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager , IConfiguration configuration, IMapper mapper , IUnitOfWork unitOfWork )
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        this._response = new APIResponse() { ErrorMessages = new List<string>() };
    }

    [HttpPost("Register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIResponse>> Register([FromBody] RegisterDTO registerDTO)
    {
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
            
            if (!await _roleManager.RoleExistsAsync(registerDTO.Role))
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("Invalid role specified.");
                return BadRequest(_response);
            }
            
            var user = _mapper.Map<ApplicationUser>(registerDTO);
            var result = await _userManager.CreateAsync(user, registerDTO.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user,registerDTO.Role);
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = $"User {user.UserName} Registered Successfully.";
                return Ok(_response);
            }
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.BadRequest;
            foreach (var error in result.Errors)
            {
                _response.ErrorMessages.Add(error.Description);
            }
            return BadRequest(_response);
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
            var roles =  await _userManager.GetRolesAsync(user);
            
            var (jwt,token) = await GenerateJwtAccessToken(user, roles);
            var refreshToken = await GenerateRefreshToken(user.Id, token.Id);
            
            await _unitOfWork.RefreshTokens.CreateAsync(refreshToken);
            await _unitOfWork.SaveChangesAsync();

            var loginResponse = new LoginResponse()
            {
                AccessToken = jwt,
                AccessTokenExpiration = token.ValidTo,
                RefreshToken = refreshToken.Token,
                RefreshTokenExpiration = refreshToken.ExpiresAt,
                UserName = user.UserName!,
                Email = user.Email!,
                Role = roles.FirstOrDefault() ?? ""
            };
            
            _response.StatusCode = HttpStatusCode.OK;
            _response.Result = loginResponse;
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.StatusCode = HttpStatusCode.InternalServerError;
            _response.ErrorMessages.Add(ex.Message);
            return StatusCode((int)HttpStatusCode.InternalServerError, _response);
        }
    }
    [HttpPost("RefreshToken")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequest model)
    {
        try
        {
            var stored = await _unitOfWork.RefreshTokens
                .GetAsync(t => t.Token == model.RefreshToken);

            if (stored == null || stored.IsRevoked || stored.ExpiresAt < DateTime.UtcNow)
            {
                return Unauthorized();
            }

            var user = await _userManager.FindByIdAsync(stored.UserId.ToString());
            if (user == null)
            {
                return Unauthorized();
            } 

            stored.IsRevoked = true;
            await _unitOfWork.RefreshTokens.UpdateAsync(stored);
            await _unitOfWork.SaveChangesAsync();

            var (newJwt, newJwtToken) = await GenerateJwtAccessToken(user, await _userManager.GetRolesAsync(user));
            var newRefresh = await GenerateRefreshToken(user.Id, newJwtToken.Id);
            await _unitOfWork.RefreshTokens.CreateAsync(newRefresh);
            await _unitOfWork.SaveChangesAsync();

            var loginResponse = new LoginResponse()
            {
                AccessToken = newJwt,
                AccessTokenExpiration = newJwtToken.ValidTo,
                RefreshToken = newRefresh.Token,
                RefreshTokenExpiration = newRefresh.ExpiresAt,
                UserName = user.UserName!,
                Email = user.Email!
            };
            _response.StatusCode = HttpStatusCode.OK;
            _response.Result = loginResponse;
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.InternalServerError;
            _response.ErrorMessages.Add(ex.Message);
        }
        return StatusCode((int)HttpStatusCode.InternalServerError, _response);
    }

    private async Task<(string token , JwtSecurityToken jwtToken)> GenerateJwtAccessToken(ApplicationUser user, IList<string> roles)
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
                Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:Key").Value!)
            );
                
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                issuer: _configuration.GetSection("JWT:Issuer").Value,
                audience: _configuration.GetSection("JWT:Audience").Value,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration.GetSection("JWT:LifeTime").Value)),
                signingCredentials: creds
            );
                
            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return (token, jwtToken);
    }
    private async Task<RefreshToken> GenerateRefreshToken(int userId, string? jwtId)
    {
        var randomBytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);

        return new RefreshToken
        {
            Token = Convert.ToBase64String(randomBytes),
            Created = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            UserId = userId,
            JwtTokenId = jwtId
        };
    }

}
