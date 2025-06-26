using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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

        public AccountHomeController(IUnitOfServices unitOfServices)
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
        public IActionResult Logout()
        {
            Response.Cookies.Delete(SD.AccessTokenKey);
            return RedirectToAction(nameof(Login));
        }
    }
}
