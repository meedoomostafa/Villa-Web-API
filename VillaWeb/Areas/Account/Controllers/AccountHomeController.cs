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
        public async Task<IActionResult> RegisterCustomer()
        {
            ViewBag.Roles = _roles;
            return View(new RegisterCustomerDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterCustomer(RegisterCustomerDTO registerCustomer)
        {
            ViewBag.Roles = _roles;
            if (!ModelState.IsValid)
            {
                return View("RegisterCustomer", registerCustomer);
            }

            var response = await _unitOfServices.AuthenticationService.RegisterCustomerAsync<APIResponse>(registerCustomer);

            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(Login));
            }
            
            var errorMessage = "Registration Failed. Please try again.";
            if (response?.ErrorMessages != null && response.ErrorMessages.Any())
            {
                errorMessage = "Registration Failed: " + string.Join(", ", response.ErrorMessages);
            }
            else if (response == null)
            {
                errorMessage = "Registration failed: An unexpected error occurred.";
            }

            ModelState.AddModelError(string.Empty, errorMessage);
            return View("RegisterCustomer", registerCustomer);
        }

        [HttpGet]
        public async Task<IActionResult> RegisterCompany()
        {
            ViewBag.Roles = _roles;
            return View(new RegisterCompanyDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterCompany(RegisterCompanyDTO registerCompany)
        {
            ViewBag.Roles = _roles;
            if (!ModelState.IsValid)
            {
                return View(nameof(RegisterCompany), registerCompany);
            }
            
            var response = await _unitOfServices.AuthenticationService.RegisterCompanyAsync<APIResponse>(registerCompany);
            
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(Login));
            }

            var errorMessage = "Registration Failed. Please try again.";
            if (response?.ErrorMessages != null && response.ErrorMessages.Any())
            {
                errorMessage = "Registration Failed: " + string.Join(", ", response.ErrorMessages);
            }
            else if (response == null)
            {
                errorMessage = "Registration failed: An unexpected error occurred.";
            }

            ModelState.AddModelError(string.Empty, errorMessage);
            return View(nameof(RegisterCompany), registerCompany);
        }

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDTO login , string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                ViewData["ReturnUrl"] = returnUrl;
                var response = await _unitOfServices.AuthenticationService
                    .LoginAsync<APIResponse>(login);
                if (response != null && response.IsSuccess && response.Result != null)
                {
                    var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(Convert.ToString(response.Result)!);

                    if (loginResponse != null && loginResponse.Id.HasValue)
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
                            new Claim(ClaimTypes.NameIdentifier, loginResponse.Id.Value.ToString()),
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
                        return RedirectToAction("Index","CustomerHome", new { area = "Customer" });
                    }
                }
                var errorMessage = "Login failed. Please check your credentials.";
                if (response?.ErrorMessages != null && response.ErrorMessages.Any())
                {
                    errorMessage = "Login failed: " + string.Join(", ", response.ErrorMessages);
                }
                else if (response == null)
                {
                    errorMessage = "Login failed: An unexpected error occurred.";
                }
                ModelState.AddModelError(string.Empty, errorMessage);
            }   
            return View("Login",login);
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
