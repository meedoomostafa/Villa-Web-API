using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VillaWeb.Models.DTOs.ProfilesDTOs;
using VillaWeb.Models.ResponseTypes;
using VillaWeb.Service.IService;
using VillaWebUtility;

namespace VillaWeb.Areas.Company.Controllers;
[Area("Company")]
[Authorize(Roles = SD.CompanyRoleName)]
public class CompanyHomeController : Controller
{
    private readonly IUnitOfServices _unitOfServices;
    private readonly IMapper _mapper;

    public CompanyHomeController(IUnitOfServices unitOfServices, IMapper mapper)
    {
        _unitOfServices = unitOfServices;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await _unitOfServices.CompanyService.GetCompanyAsync<APIResponse>(userId);

        if (response is { IsSuccess: true, Result: not null })
        {
            var company = JsonConvert.DeserializeObject<CompanyProfileDTO>(Convert.ToString(response.Result)!);
            return View(company);
        }

        TempData["ErrorMessage"] = "Could not retrieve your company profile. Please try again later.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Profile(CompanyProfileDTO model)
    {
        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Invalid data provided.";
            return View(model);
        }

        var response = await _unitOfServices.CompanyService
            .UpdateCompanyAsync<APIResponse>(model);

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

}
