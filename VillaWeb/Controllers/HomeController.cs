using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VillaWeb.Models;
using VillaWeb.Models.DTOs.VillaDTOs;
using VillaWeb.Models.ResponseTypes;
using VillaWeb.Service.IService;

namespace VillaWeb.Controllers;

public class HomeController : Controller
{
    private readonly IUnitOfServices _unitOfServices;
    public HomeController(IUnitOfServices unitOfServices)
    {
        _unitOfServices = unitOfServices;
    }

    public async Task<IActionResult> Index()
    {
        List<VillaDTO> villas = new List<VillaDTO>();
        var token = Request.Cookies["AuthToken"];
        var response = await _unitOfServices.VillaService.GetAllAsync<APIResponse>(token!);
        if (response != null && response.IsSuccess)
        {
            villas = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result));
        }
        return View(villas);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
