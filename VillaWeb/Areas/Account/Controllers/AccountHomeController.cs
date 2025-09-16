using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using VillaWeb.Models;
using VillaWeb.Models.DTOs.AuthenticationDTOs;
using VillaWeb.Models.ResponseTypes;
using VillaWeb.Service.IService;
using VillaWebUtility;

namespace VillaWeb.Areas.Account.Controllers
{
    [Area("Account")]
    public class AccountHomeController : Controller
    {
        private readonly IUnitOfServices _unitOfServices;
        private readonly IEnumerable<SelectListItem> _roles;

        public AccountHomeController(IUnitOfServices unitOfServices, IOptions<List<RoleItem>> rolesOptions)
        {
            _unitOfServices = unitOfServices;
            _roles = rolesOptions.Value.Select(u => new SelectListItem()
            {
                Value = u.Value,
                Text = u.Text
            });
        }
        
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            ViewBag.Roles = _roles;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDTO register)
        {
            ViewBag.Roles = _roles;
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
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDTO login , string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                ViewData["ReturnUrl"] = returnUrl;
                var response = await _unitOfServices.AuthenticationService.LoginAsync<APIResponse>(login);
                if (response != null && response.IsSuccess && response.Result != null)
                {
                    var loginResponse = JsonConvert
                        .DeserializeObject<LoginResponse>(Convert.ToString(response.Result)!);

                    if (loginResponse != null)
                    {
                        Response.Cookies.Append(SD.AccessTokenKey, loginResponse.AccessToken, new CookieOptions()
                        {
                            HttpOnly = true,
                            Secure = true,
                            SameSite = SameSiteMode.Strict,
                            Expires = loginResponse.AccessTokenExpiration
                        });
                        Response.Cookies.Append(SD.RefreshTokenKey, loginResponse.RefreshToken, new CookieOptions()
                        {
                            HttpOnly = true,
                            Secure = true,
                            SameSite = SameSiteMode.Strict,
                            Expires = loginResponse.RefreshTokenExpiration
                        });
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, loginResponse.UserName),
                            new Claim(ClaimTypes.Role, loginResponse.Role), 
                        };

                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                            new AuthenticationProperties
                            {
                                IsPersistent = true,
                                ExpiresUtc = loginResponse.RefreshTokenExpiration
                            });

                        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        return RedirectToAction(nameof(Index),"CustomerHome", new { area = "Customer" });
                    }
                }

                ModelState.AddModelError("",
                    "Login failed" + string.Join(", ", response?.ErrorMessages ?? new List<string>()));
            }   
            return View(login);
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete(SD.AccessTokenKey);
            Response.Cookies.Delete(SD.RefreshTokenKey);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }
    }
}
