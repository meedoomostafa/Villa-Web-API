using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VillaWeb.Models.DTOs.VillaDTOs;
using VillaWeb.Models.ResponseTypes;
using VillaWeb.Service.IService;

namespace VillaWeb.Controllers;

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
        var token = Request.Cookies["AuthToken"];
        var response = await _unitOfServices.VillaService.GetAllAsync<APIResponse>(token!);
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
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(VillaCreateDTO villa , IFormFile? imageFile)
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
            
            var token = Request.Cookies["AuthToken"];
            var response = await _unitOfServices.VillaService.CreateAsync<APIResponse>(villa,token!);
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
        var token = Request.Cookies["AuthToken"];
        var response = await _unitOfServices.VillaService.GetAsync<APIResponse>(id,token!);
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
            
            var token = Request.Cookies["AuthToken"];
            var response = await _unitOfServices.VillaService.UpdateAsync<APIResponse>(villa,token!);
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
        var token = Request.Cookies["AuthToken"];
        var response = await _unitOfServices.VillaService.GetAsync<APIResponse>(id,token!);
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
        var token = Request.Cookies["AuthToken"];
        var getResponse = await _unitOfServices.VillaService.GetAsync<APIResponse>(id,token!);
        if (getResponse == null || !getResponse.IsSuccess)
        {
            TempData["error"] = "Villa not found.";
            return RedirectToAction(nameof(Index));
        }

        var villa = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(getResponse.Result));

        var response = await _unitOfServices.VillaService.DeleteAsync<APIResponse>(id,token!);
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
}
