using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using VillaWebApi.Models.DTOs.VillaDTOs;
using VillaWebApi.Models;
using VillaWebApi.Repository.Interfaces;

namespace VillaWebApi.Controllers;

// [Route("api/[controller]")]
[Route("api/VillaApi")]
[ApiController]
public class VillaApiController : ControllerBase
{
    protected APIResponse _response;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public VillaApiController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        this._response = new APIResponse()
        {
            ErrorMessages = new List<string>()
        };
    }

    [HttpGet("GetAllVilla")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIResponse>> GetAllVillas()
    {
        try
        {
            List<Villa> villas = await _unitOfWork.Villa.GetAllAsync();
            if (villas == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("No Villas found");
                return NotFound(_response);
            }
            _response.Result = _mapper.Map<List<VillaDTO>>(villas);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
        catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages.Add(e.Message);
            _response.StatusCode = HttpStatusCode.InternalServerError;
        }
        return StatusCode((int)HttpStatusCode.InternalServerError,_response);
    }

    [HttpGet("GetVilla/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIResponse>> GetVilla(int id)
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

            Villa? villa = await _unitOfWork.Villa.GetAsync(u => u.Id == id);
            if (villa == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("No Villa found");
                return NotFound(_response);
            }
            _response.Result = _mapper.Map<VillaDTO>(villa);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
        catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages.Add(e.Message);
            _response.StatusCode = HttpStatusCode.InternalServerError;
        }
        return StatusCode((int) HttpStatusCode.InternalServerError, _response);
    }

    [HttpPost("CreateVilla")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIResponse>> CreateVilla([FromBody] VillaCreateDTO createDTO)
    {
        try
        {
            if (createDTO == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("Null Villa");
                return BadRequest(_response);
            }

            Villa? checkVillaExistence = await _unitOfWork.Villa
                .GetAsync(u => u.Name.ToLower() == createDTO.Name.ToLower());
        
            if (checkVillaExistence != null)
            {
                ModelState.AddModelError("Name", "Villa name already exists");
                return BadRequest(ModelState);
            }

            Villa model = _mapper.Map<Villa>(createDTO);

            await _unitOfWork.Villa.CreateAsync(model);
            await _unitOfWork.SaveChangesAsync();
            _response.Result = _mapper.Map<VillaDTO>(model);
            _response.StatusCode = HttpStatusCode.Created;
            return CreatedAtAction("GetVilla", new { id = model.Id }, _response);
        }
        catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages.Add(e.Message);
            _response.StatusCode = HttpStatusCode.InternalServerError;
        }
        return StatusCode((int) HttpStatusCode.InternalServerError, _response);
    }

    [HttpPut("UpdateVilla/{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIResponse>> UpdateVilla(int id, VillaUpdateDTO updateDTO)
    {
        try
        {
            if (id == 0 || updateDTO == null || id != updateDTO.Id)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("Wrong ID or Null Villa");
                return BadRequest(_response);
            }
            
            var checkVillaExistence = await _unitOfWork.Villa.GetAsync(u => u.Id == id);
            if (checkVillaExistence == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("No Villa found with the specified ID");
                return NotFound(_response);
            }
            
            Villa model = _mapper.Map<Villa>(updateDTO);
            await _unitOfWork.Villa.UpdateAsync(model);
            await _unitOfWork.SaveChangesAsync();
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
        catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages.Add(e.Message); 
            _response.StatusCode = HttpStatusCode.InternalServerError;
        }
        return StatusCode((int)HttpStatusCode.InternalServerError, _response);
    }

    [HttpPatch("UpdatePartialVilla/{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APIResponse>> UpdatePartialVilla(int id, [FromBody] JsonPatchDocument<VillaUpdateDTO> villa)
    {
        if (villa == null || id == 0)
        {
            return BadRequest();
        }

        Models.Villa? villaFromDb = await _unitOfWork.Villa.GetAsync(u => u.Id == id,false);
        if (villaFromDb == null)
        {
            return NotFound("No Villa found");
        }

        VillaUpdateDTO updateDTO = _mapper.Map<VillaUpdateDTO>(villaFromDb);
        villa.ApplyTo(updateDTO, ModelState);
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        _mapper.Map(updateDTO, villaFromDb);
        await _unitOfWork.Villa.UpdateAsync(villaFromDb);
        await _unitOfWork.SaveChangesAsync();

        return Ok(villaFromDb);
    }

    [HttpDelete("DeleteVilla/{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIResponse>> DeleteVilla(int id)
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

            Villa? villa = await _unitOfWork.Villa.GetAsync(u => u.Id == id);
            if (villa == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("No Villa found with the specified ID");
                return NotFound(_response);
            }

            await _unitOfWork.Villa.RemoveAsync(villa);
            await _unitOfWork.SaveChangesAsync();
            _response.StatusCode = HttpStatusCode.NoContent;
            return Ok(_response);
        }
        catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages.Add(e.Message);
            _response.StatusCode = HttpStatusCode.InternalServerError;
        }
        return StatusCode((int)HttpStatusCode.InternalServerError,_response);
    }
}
