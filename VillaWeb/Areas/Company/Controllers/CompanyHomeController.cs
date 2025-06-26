using Microsoft.AspNetCore.Mvc;

namespace VillaWeb.Areas.Company.Controllers
{
    [Area("Company")]
    public class CompanyHomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
