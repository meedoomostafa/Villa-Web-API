using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VillaWebApi.Models;
using VillaWebApi.Models.DTOs.VillaNumberDTOs;
using VillaWebApi.Repository;
using VillaWebApi.Repository.Interfaces;

namespace VillaWebApi.Controllers;

[Route("api/VillaNumberApi")]
[ApiController]
public class VillaNumberApiController : ControllerBase
{
    protected APIResponse _response;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public VillaNumberApiController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _response = new APIResponse()
        {
            ErrorMessages = new List<string>()
        };
    }

    [HttpGet("GetAllVillaNumbers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIResponse>> GetAllVillaNumbers()
    {
        try
        {
            List<VillaNumber> villaNumbers = await _unitOfWork.VillaNumber.GetAllAsync();
            if (villaNumbers == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("No villa-numbers found");
                return NotFound(_response);
            }
            _response.Result = _mapper.Map<List<VillaNumberDTO>>(villaNumbers);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
        catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.InternalServerError;
            _response.ErrorMessages.Add(e.Message);
        }
        return StatusCode((int)HttpStatusCode.InternalServerError,_response);
    }

    [HttpGet("GetVillaNumber/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIResponse>> GetVillaNumber(int id)
    {
        try
        {
            if (id == 0)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("Wrong ID");
                return BadRequest(_response);
            }
            var villaNumber = await _unitOfWork.VillaNumber.GetAsync(u => u.VillaNo == id);
            if (villaNumber == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("No villa-number found");
            }
            _response.Result = _mapper.Map<VillaNumber>(villaNumber);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
        catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.InternalServerError;
            _response.ErrorMessages.Add(e.Message);
        }
        return StatusCode((int)HttpStatusCode.InternalServerError, _response);
    }

    [HttpPost("CreateVillaNumber")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIResponse>> CreateVillaNumber([FromBody] VillaNumberCreateDTO CreateDTO)
    {
        try
        {
            if (CreateDTO == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("No villa-number found");
                return BadRequest(_response);
            }
            
            var checkVillaNumberExistence = await _unitOfWork.VillaNumber
                .GetAsync(u => u.VillaNo  == CreateDTO.VillaNo);
            if (checkVillaNumberExistence != null)
            {
                ModelState.AddModelError("Name", "Villa number already exists");
                return BadRequest(ModelState);
            }
            
            VillaNumber villaNumber = _mapper.Map<VillaNumber>(CreateDTO);
            await _unitOfWork.VillaNumber.CreateAsync(villaNumber);
            await _unitOfWork.SaveChangesAsync();
            _response.StatusCode = HttpStatusCode.Created;
            return CreatedAtAction("GetVillaNumber" , new {id = villaNumber.VillaNo} , _response);
        }
        catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.InternalServerError;
            _response.ErrorMessages.Add(e.Message);
        }
        return StatusCode((int)HttpStatusCode.InternalServerError, _response);
    }

    [HttpPut("UpdateVillaNumber/{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIResponse>> UpdateVillaNumber(int id,VillaNumberUpdateDTO UpdateDTO)
    {
        try
        {
            if (id == 0 || UpdateDTO == null || UpdateDTO.VillaNo != id )
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("No villa-number found");
                return BadRequest(_response);
            }
            
            VillaNumber villaNumber = _mapper.Map<VillaNumber>(UpdateDTO);
            await _unitOfWork.VillaNumber.UpdateAsync(villaNumber);
            await _unitOfWork.SaveChangesAsync();
            _response.StatusCode = HttpStatusCode.NoContent;
            return Ok(_response);
        }
        catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.InternalServerError;
            _response.ErrorMessages.Add(e.Message);
        }
        return StatusCode((int)HttpStatusCode.InternalServerError, _response);
    }

    [HttpDelete("DeleteVillaNumber/{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIResponse>> DeleteVillaNumber(int id)
    {
        try
        {
            if (id == 0)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("Invalid ID");
                return BadRequest(_response);
            }
        
            var villaNumber = await _unitOfWork.VillaNumber.GetAsync(u => u.VillaNo == id);
            if (villaNumber == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("No villa-number found");
                return NotFound(_response);
            }
            await _unitOfWork.VillaNumber.RemoveAsync(villaNumber);
            await _unitOfWork.SaveChangesAsync();
            _response.StatusCode = HttpStatusCode.NoContent;
            return Ok(_response);
        }
        catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.InternalServerError;
            _response.ErrorMessages.Add(e.Message);
        }
        return StatusCode((int)HttpStatusCode.InternalServerError, _response);
    }
}
