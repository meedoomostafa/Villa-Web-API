using System.Diagnostics;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VillaWeb.Models;
using VillaWeb.Models.DTOs.ProfilesDTOs;
using VillaWeb.Models.DTOs.VillaDTOs;
using VillaWeb.Models.ResponseTypes;
using VillaWeb.Service.IService;

namespace VillaWeb.Areas.Customer.Controllers;

[Area("Customer")]
public class CustomerHomeController : Controller
{
    private readonly IUnitOfServices _unitOfServices;
    private readonly IMapper _mapper;

    public CustomerHomeController(IUnitOfServices unitOfServices, IMapper mapper)
    {
        _unitOfServices = unitOfServices;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        List<VillaDTO> villas = new List<VillaDTO>();
        var response = await _unitOfServices.VillaService.GetAllAsync<APIResponse>();
        if (response != null && response.IsSuccess)
        {
            villas = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result));
        }
        return View(villas);
    }
    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await _unitOfServices.CustomerService
            .GetCustomerAsync<APIResponse>(userId);

        if (response is { IsSuccess: true, Result: not null })
        {
            var customer = JsonConvert.DeserializeObject<CustomerProfileDTO>(Convert.ToString(response.Result)!);
            var customerUpdateDto = _mapper.Map<CustomerProfileDTO>(customer);
            return View(customerUpdateDto);
        }

        TempData["ErrorMessage"] = "Could not retrieve your profile. Please try again later.";
        return RedirectToAction("Index", "CustomerHome");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Profile(CustomerProfileDTO model)
    {
        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Invalid data provided.";
            return View(model);
        }

        var response = await _unitOfServices.CustomerService.UpdateCustomerAsync<APIResponse>(model);

        if (response is { IsSuccess: true })
        {
            TempData["SuccessMessage"] = "Your profile has been updated successfully!";
        }
        else
        {
            TempData["ErrorMessage"] = "There was an error updating your profile.";
        }

        return RedirectToAction(nameof(Profile));
    }

    [Authorize]
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