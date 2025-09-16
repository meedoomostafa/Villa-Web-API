using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VillaWebUtility;

namespace VillaWeb.Areas.Company.Controllers
{
    [Area("Company")]
    [Authorize(Roles = SD.CompanyRoleName)]
    public class CompanyHomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
