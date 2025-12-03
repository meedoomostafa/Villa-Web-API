using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using VillaWeb.Models.DTOs.VillaDTOs;
using VillaWeb.Models.ResponseTypes;
using VillaWeb.Service.IService;
using VillaWebUtility;

namespace VillaWeb.Areas.Company.Controllers;

[Area("Company")]
[Authorize(Roles = SD.CompanyRoleName)]
public class VillaController : Controller
{
    private readonly IUnitOfServices  _unitOfServices;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _env;
    public VillaController(IUnitOfServices unitOfServices, IMapper mapper , IWebHostEnvironment env)
    {
        _unitOfServices = unitOfServices;
        _mapper = mapper;
        _env = env;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        List<VillaDTO> list = new List<VillaDTO>();
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await _unitOfServices.VillaService.GetAllCompanyVillas<APIResponse>(userId);
        if (response != null && response.IsSuccess)
        {
            list = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result));
        }
        return View(list);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var idClaim = User?
            .FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(idClaim, out var currentUserId))
        {
            return Challenge(); 
        }

        var villa = new VillaCreateDTO { CompanyId = currentUserId };
        return View(villa);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(VillaCreateDTO villa ,[FromForm] IFormFile? imageFile)
    {
        if (ModelState.IsValid)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var extension = Path.GetExtension(imageFile.FileName).ToLower();
                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("Image File", "Invalid file extension.");
                    return View(villa);
                }
                if (imageFile.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError("imageFile", "Max size is 5MB.");
                    return View(villa);
                }

                var fileName = Guid.NewGuid().ToString() + extension;

                var uploadPath = Path.Combine(_env.WebRootPath, "Images");

                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                var filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                villa.ImageUrl = "/Images/" + fileName;
            }
            else
            {
                ModelState.AddModelError("Image File", "Please upload an image file.");
                return View(villa);
            }
            
            var response = await _unitOfServices.VillaService.CreateAsync<APIResponse>(villa);
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }
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
    public async Task<IActionResult> Edit(VillaUpdateDTO villa , IFormFile? imageFile)
    {
        if (ModelState.IsValid)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var extension = Path.GetExtension(imageFile.FileName).ToLower();
                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("Image File", "Invalid file extension.");
                    return View(villa);
                }
                if (imageFile.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError("Image File", "Max file size is 5MB.");
                    return View(villa);
                }

                var fileName = Guid.NewGuid().ToString() + extension;
                var uploadPath = Path.Combine(_env.WebRootPath, "Images");

                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                var filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                villa.ImageUrl = "/Images/" + fileName;
            }
            
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
        var getResponse = await _unitOfServices.VillaService.GetAsync<APIResponse>(id);
        if (getResponse == null || !getResponse.IsSuccess)
        {
            TempData["error"] = "Villa not found.";
            return RedirectToAction(nameof(Index));
        }

        var villa = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(getResponse.Result));

        var response = await _unitOfServices.VillaService.DeleteAsync<APIResponse>(id);
        if (response != null && response.IsSuccess)
        {
            if (!string.IsNullOrEmpty(villa!.ImageUrl))
            {
                var imagePath = Path.Combine(_env.WebRootPath, villa.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            TempData["success"] = "Villa deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        TempData["error"] = "Error while deleting!";
        return RedirectToAction(nameof(Index));
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var villa = new VillaWithVillaNumbersDTO();
        var response = await _unitOfServices.VillaService.GetVillaWithVillaNumberAsync<APIResponse>(id);
        if (response != null && response.IsSuccess)
        {
            villa = JsonConvert.DeserializeObject<VillaWithVillaNumbersDTO>(Convert.ToString(response.Result)!);
            villa.VillaNumbersSelectList = villa.VillaNumbers.Select(vn => new SelectListItem
            {
                Value = vn.Id.ToString(),
                Text = $"Villa Number: {vn.Id}"
            }).ToList();
            
            TempData["ReturnUrl"] = Url.Action("Details", "Villa", new { area = "Company" , id = villa.Id });
            return View(villa);
        }
        return NotFound();
    }
}
