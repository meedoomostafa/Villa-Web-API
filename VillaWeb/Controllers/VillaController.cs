using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VillaWeb.Models;
using VillaWeb.Models.DTOs.VillaDTOs;
using VillaWeb.Service.IService;

namespace VillaWeb.Controllers;

public class VillaController : Controller
{
    private readonly IUnitOfServices  _unitOfServices;
    private readonly IMapper _mapper;
    public VillaController(IUnitOfServices unitOfServices, IMapper mapper)
    {
        _unitOfServices = unitOfServices;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        List<VillaDTO> list = new List<VillaDTO>();
        var response = await _unitOfServices.VillaService.GetAllAsync<APIResponse>();
        if (response != null && response.IsSuccess)
        {
            list = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result));
        }
        return View(list);
    }

    public async Task<IActionResult> Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(VillaCreateDTO villa)
    {
        if (ModelState.IsValid)
        {
            var response = await _unitOfServices.VillaService.CreateAsync<APIResponse>(villa);
            return RedirectToAction(nameof(Index));
        }
        return View(villa);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        VillaDTO villa = new VillaDTO();
        var response = await _unitOfServices.VillaService.GetAsync<APIResponse>(id);
        if (response != null && response.IsSuccess)
        {
            villa = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response.Result));
        }
        return View(_mapper.Map<VillaUpdateDTO>(villa));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(VillaUpdateDTO villa)
    {
        if (ModelState.IsValid)
        {
            var response = await _unitOfServices.VillaService.UpdateAsync<APIResponse>(villa);
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
        VillaDTO villa = new VillaDTO();
        var response = await _unitOfServices.VillaService.GetAsync<APIResponse>(id);
        if (response != null && response.IsSuccess)
        {
            villa = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response.Result));
            return View(villa); 
        }
        return NotFound();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirm(int id)
    {
        var response = await _unitOfServices.VillaService.DeleteAsync<APIResponse>(id);
        if (response != null && response.IsSuccess)
        {
            return RedirectToAction(nameof(Index));
        }
        return NotFound();
    }
}
