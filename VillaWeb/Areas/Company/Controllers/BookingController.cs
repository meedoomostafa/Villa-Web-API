using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VillaWeb.Models.DTOs.BookingDTOs;
using VillaWeb.Models.ResponseTypes;
using VillaWeb.Service.IService;

namespace VillaWeb.Areas.Company.Controllers;

[Area("Company")]
public class BookingController : Controller
{
    private readonly IUnitOfServices  _unitOfServices;
    private readonly IMapper _mapper;

    public BookingController(IUnitOfServices unitOfServices , IMapper mapper)
    {
        _unitOfServices = unitOfServices;
        _mapper = mapper;
    }
    
    public async Task<IActionResult> Index()
    {
        List<BookingDTO> bookings = new List<BookingDTO>();
        var response = await _unitOfServices.BookingService.GetAllAsync<APIResponse>();
        if (response != null && response.IsSuccess)
        {
            bookings = JsonConvert.DeserializeObject<List<BookingDTO>>(Convert.ToString(response.Result)!)!;
        }
        return View(bookings);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var booking = new BookingDTO();
        var response = await _unitOfServices.BookingService.GetAsync<APIResponse>(id);
        if (response != null && response.IsSuccess)
        {
            booking = JsonConvert.DeserializeObject<BookingDTO>(Convert.ToString(response.Result)!);
        }
        return View(booking);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var booking = new BookingDTO();
        var response = await _unitOfServices.BookingService.GetAsync<APIResponse>(id);
        if (response != null && response.IsSuccess)
        {
            booking = JsonConvert.DeserializeObject<BookingDTO>(Convert.ToString(response.Result)!);
        }
        return View(_mapper.Map<BookingUpdateDTO>(booking));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(BookingUpdateDTO booking)
    {
        var response = await _unitOfServices.BookingService.UpdateAsync<APIResponse>(booking);
        if (response != null && response.IsSuccess)
        {
            return RedirectToAction(nameof(Index));
        }
        return View(booking);
    }
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BookingCreateDTO booking , string returnUrl = null)
    {
        if (!ModelState.IsValid)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                TempData["BookingErrors"] = JsonConvert.SerializeObject(
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                );
                return Redirect(returnUrl);
            }

            return View(booking);
        }
        var response = await _unitOfServices.BookingService.CreateAsync<APIResponse>(booking);
        if (response != null && !response.IsSuccess)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                TempData["BookingErrors"] = JsonConvert.SerializeObject(response.ErrorMessages);
                return Redirect(returnUrl);
            }
            foreach (var error in response.ErrorMessages)
            {
                ModelState.AddModelError("",error);
            }
            return View(booking);
        }
        if (response != null && response.IsSuccess)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                TempData["SuccessMessage"] =  "Booking was successfully created";
                return Redirect(returnUrl);
            }
            return RedirectToAction(nameof(Index));
        }
        ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
        return View(booking);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var booking = new BookingDTO();
        var response = await _unitOfServices.BookingService.GetAsync<APIResponse>(id);
        if (response != null && response.IsSuccess)
        {
            booking = JsonConvert.DeserializeObject<BookingDTO>(Convert.ToString(response.Result)!);
        }
        return View(booking);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirm(int id)
    {
        var response = await _unitOfServices.BookingService.DeleteAsync<APIResponse>(id);
        if (response != null && response.IsSuccess)
        {
            return RedirectToAction(nameof(Index));
        }
        return NotFound();
    }
}