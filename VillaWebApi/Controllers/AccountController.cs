using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using VillaModels.Models;
using VillaModels.Models.DTOs.AuthenticationDTOs;

namespace VillaWebApi.Controllers;

[Route("api/Account")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    private readonly APIResponse _response;

    public AccountController(UserManager<ApplicationUser> userManager, IConfiguration configuration, IMapper mapper )
    {
        _userManager = userManager;
        _configuration = configuration;
        _mapper = mapper;
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
            
            var user = _mapper.Map<ApplicationUser>(registerDTO);
            var result = await _userManager.CreateAsync(user, registerDTO.Password);
            if (result.Succeeded)
            {
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
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!),
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:Key").Value!)
            );
            
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration.GetSection("JWT:Issuer").Value,
                audience: _configuration.GetSection("JWT:Audience").Value,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration.GetSection("JWT:LifeTime").Value)),
                signingCredentials: creds
            );
            
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            var loginResponse = new LoginResponse()
            {
                Token = jwt,
                Expiration = token.ValidTo,
                UserName = user.UserName!,
                Email = user.Email!,
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
}
