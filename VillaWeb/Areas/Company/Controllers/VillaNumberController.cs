using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VillaWeb.Models.DTOs.VillaNumberDTOs;
using VillaWeb.Models.ResponseTypes;
using VillaWeb.Service.IService;

namespace VillaWeb.Areas.Company.Controllers;

[Area("Company")]
public class VillaNumberController  : Controller
{
    private readonly IUnitOfServices _unitOfServices;
    private readonly IMapper _mapper;

    public VillaNumberController(IUnitOfServices unitOfServices, IMapper mapper)
    {
        _unitOfServices = unitOfServices;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        List<VillaNumberDTO> villasNumber = new List<VillaNumberDTO>();
        var response = await _unitOfServices.VillaNumberService.GetAllAsync<APIResponse>();
        if (response != null && response.IsSuccess)
        {
            villasNumber = JsonConvert.DeserializeObject<List<VillaNumberDTO>>(Convert.ToString(response.Result));
        }
        return View(villasNumber);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var villa = new  VillaNumberDTO();
        var response = await _unitOfServices.VillaNumberService.GetAsync<APIResponse>(id);
        if (response != null && response.IsSuccess)
        {
            villa = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(response.Result));
        }
        return View(_mapper.Map<VillaNumberUpdateDTO>(villa));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(VillaNumberUpdateDTO villa)
    {
        if (ModelState.IsValid)
        {
            var response = await _unitOfServices.VillaNumberService.UpdateAsync<APIResponse>(villa);
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }
        }
        return View(villa);
    }
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(VillaNumberCreateDTO villa)
    {
        if (ModelState.IsValid)
        {
            var response = await _unitOfServices.VillaNumberService.CreateAsync<APIResponse>(villa);
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }
        }
        return View(villa);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var villaNumber = new VillaNumberDTO();
        var response = await _unitOfServices.VillaNumberService.GetAsync<APIResponse>(id);
        if (response != null && response.IsSuccess)
        {
            villaNumber = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(response.Result));
        }
        return View(villaNumber);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirm(int id)
    {
        var response = await _unitOfServices.VillaNumberService.DeleteAsync<APIResponse>(id);
        if (response != null && response.IsSuccess)
        {
            return RedirectToAction(nameof(Index));
        }
        return NotFound();
    }
}
