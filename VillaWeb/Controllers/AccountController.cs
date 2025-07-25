using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VillaWeb.Models.DTOs.AuthenticationDTOs;
using VillaWeb.Models.ResponseTypes;
using VillaWeb.Service.IService;

namespace VillaWeb.Controllers;

public class AccountController : Controller
{
    private readonly IUnitOfServices _unitOfServices;

    public AccountController(IUnitOfServices unitOfServices)
    {
        _unitOfServices = unitOfServices;
    }
    
    [HttpGet]
    public async Task<IActionResult> Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterDTO register)
    {
        if (ModelState.IsValid)
        {
            var response = await _unitOfServices
                .AuthenticationService.RegisterAsync<APIResponse>(register);
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(Login));
            }

            ModelState.AddModelError(string.Empty
                , "Registration Failed" +
                  string.Join(", ", response?.ErrorMessages ?? new List<string>()));
        }
        return View(nameof(Register),register);
    }
    [HttpGet]
    public async Task<IActionResult> Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginDTO login)
    {
        if (ModelState.IsValid)
        {
            var response = await _unitOfServices.AuthenticationService.LoginAsync<APIResponse>(login);
            if (response != null && response.IsSuccess && response.Result != null)
            {
                var loginResponse = JsonConvert
                    .DeserializeObject<LoginResponse>(Convert.ToString(response.Result)!);

                if (loginResponse != null)
                {
                    Response.Cookies.Append("AuthToken", loginResponse.Token, new CookieOptions()
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = loginResponse.Expiration
                    });
                    return RedirectToAction(nameof(Index),"Home");
                }
            }

            ModelState.AddModelError("",
                "Login failed" + string.Join(", ", response?.ErrorMessages ?? new List<string>()));
        }   
        return View(login);
    }
    [HttpGet]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("AuthToken");
        return RedirectToAction(nameof(Login));
    }
}