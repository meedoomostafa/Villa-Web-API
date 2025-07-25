using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VillaModels.Models;
using VillaModels.Models.DTOs.BookingDTOs;
using VillaRepository.Repository.Interfaces;

namespace VillaWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookingController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    protected APIResponse _response;

    public BookingController(IUnitOfWork unitOfWork ,  IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        this._response = new  APIResponse() { ErrorMessages = new List<string>() };
    }

    [HttpGet(Name = "GetAllBookings")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIResponse>> GetAllBookings()
    {
        try
        {
            var bookings = await _unitOfWork.Booking
                .GetAllAsync(include: q => q.Include(b => b.ApplicationUser)
                    .Include(b => b.VillaNumber)
                    .ThenInclude(v => v.Villa));
            
            if (bookings == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("No Bookings found");
                return NotFound(_response);
            }
            _response.StatusCode = HttpStatusCode.OK;
            _response.Result = _mapper.Map<List<BookingDTO>>(bookings);
            return Ok(_response);
        }
        catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.InternalServerError;
            _response.ErrorMessages.Add(e.Message);
        }
        return StatusCode((int)_response.StatusCode, _response);
    }

    [HttpGet("{id:int}", Name = "GetBooking")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIResponse>> GetBooking(int id)
    {
        try
        {
            var booking = await _unitOfWork.Booking
                .GetAsync(b => b.Id == id,include: q => q.Include(b => b.ApplicationUser)
                    .Include(b => b.VillaNumber)
                    .ThenInclude(v => v.Villa));
            if (booking == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("Booking not found");
                return NotFound(_response);
            }

            _response.StatusCode = HttpStatusCode.OK;
            _response.Result = _mapper.Map<BookingDTO>(booking);
            return Ok(_response);
        }
        catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.InternalServerError;
            _response.ErrorMessages.Add(e.Message);
        }
        return StatusCode((int)_response.StatusCode, _response);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIResponse>> CreateBooking(BookingCreateDTO booking)
    {
        try
        {
            if (booking == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("Booking not found");
                return BadRequest(_response);
            }
            if (!ModelState.IsValid)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("Invalid Data entered");
                return BadRequest(_response);
            }
            var bookingEntity = _mapper.Map<Booking>(booking);
            await _unitOfWork.Booking.CreateAsync(bookingEntity);
            await _unitOfWork.SaveChangesAsync();
            _response.StatusCode = HttpStatusCode.Created;
            return CreatedAtRoute("GetBooking", new { id = bookingEntity.Id }, _response);
        }
        catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.InternalServerError;
            _response.ErrorMessages.Add(e.Message);
        }
        return StatusCode((int)_response.StatusCode, _response);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIResponse>> UpdateBooking(BookingUpdateDTO booking)
    {
        try
        {
            if (booking.Id == 0 || booking.Id == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("Incorrect Id");
                return BadRequest(_response);
            }

            var bookingEntity = await _unitOfWork.Booking
                .GetAsync(b => b.Id == booking.Id);

            if (bookingEntity == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("Booking not found");
                return NotFound(_response);
            }

            _mapper.Map(booking, bookingEntity);
            await _unitOfWork.SaveChangesAsync();
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
        catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.InternalServerError;
            _response.ErrorMessages.Add(e.Message);
        }        
        return StatusCode((int)_response.StatusCode, _response);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIResponse>> DeleteBooking(int id)
    {
        if (id == 0)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.ErrorMessages.Add("Incorrect Id");
            return BadRequest(_response);
        }
        var bookingEntity = await _unitOfWork.Booking
            .GetAsync(b => b.Id == id);
        if (bookingEntity == null)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.NotFound;
            _response.ErrorMessages.Add("Booking not found");
            return NotFound(_response);
        }
        await _unitOfWork.Booking.RemoveAsync(bookingEntity);
        await _unitOfWork.SaveChangesAsync();
        _response.StatusCode = HttpStatusCode.OK;
        return Ok(_response);
    }
}