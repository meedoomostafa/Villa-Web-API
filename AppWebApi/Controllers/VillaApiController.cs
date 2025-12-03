using System.Net;
using AppService.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using AppModels.Models;
using AppModels.Models.DTOs.VillaDTOs;

namespace AppWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize(Roles = ApplicationRoles.CompanyRoleName)]
public class VillaApiController : ControllerBase
{
    protected APIResponse _response;
    private readonly IUnitOfServices _unitOfServices;
    private readonly IMapper _mapper;

    public VillaApiController(IUnitOfServices unitOfServices, IMapper mapper)
    {
        _unitOfServices = unitOfServices;
        _mapper = mapper;
        this._response = new APIResponse() { ErrorMessages = new List<string>() };
    }

    [AllowAnonymous]
    [HttpGet(Name = "GetAllVillas")]
    [ResponseCache(CacheProfileName = "30s")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIResponse>> GetAllVillas()
    {
        try
        {
            List<Villa> villas = await _unitOfServices.Villas.GetAllVillas();
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

    [HttpGet("GetAllCompanyVillas/{id:int}")]
    [ResponseCache(CacheProfileName = "30s")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIResponse>> GetAllCompanyVillas(int id)
    {
        try
        {
            List<Villa> villas = await _unitOfServices.Villas.GetAllCompanyVillas(id);
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
    
    [AllowAnonymous]
    [HttpGet("GetVillaWithVillaNumbers/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIResponse>> GetVillaWithVillaNumbers(int id)
    {
        try
        {
            var villa = await _unitOfServices.Villas.GetVillaWithVillaNumbers(id);
            if (villa == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("No Villas found");
                return NotFound(_response);
            }
            _response.Result = _mapper.Map<VillaWithVillaNumbersDTO>(villa);
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

    [HttpGet("{id:int}" , Name = "GetVilla")]
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

            Villa? villa = await _unitOfServices.Villas.GetVilla(id);
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

    [HttpPost(Name = "CreateVilla")]
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

            var checkVillaNameExistence = await _unitOfServices.Villas
                .CheckVillaNameExistence(createDTO.Name);
            
            if (checkVillaNameExistence != null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("Villa already exists");
                return BadRequest(_response);
            }

            var checkCompanyExistence = await _unitOfServices.Companies
                .CheckCompanyExistence(createDTO.CompanyId);
            if (checkCompanyExistence == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("Company Id doesn't exist");
                return BadRequest(_response);
            }

            var villa = _mapper.Map<Villa>(createDTO);
            await _unitOfServices.Villas.CreateVilla(villa);
            await _unitOfServices.SaveChangesAsync();
            
            _response.Result = _mapper.Map<VillaDTO>(villa);
            _response.StatusCode = HttpStatusCode.Created;
            return CreatedAtAction("GetVilla", new { id = villa.Id }, _response);
        }
        catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages.Add(e.Message);
            _response.StatusCode = HttpStatusCode.InternalServerError;
        }
        return StatusCode((int) HttpStatusCode.InternalServerError, _response);
    }

    [HttpPut("{id:int}" , Name = "UpdateVilla")]
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
            
            var villaFromDb = await _unitOfServices.Villas.GetVilla(id);
            if (villaFromDb == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("No Villa found with the specified ID");
                return NotFound(_response);
            }

            _mapper.Map(updateDTO, villaFromDb);
            await _unitOfServices.Villas.UpdateVilla(villaFromDb);
            await _unitOfServices.SaveChangesAsync();
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

    [HttpPatch("{id:int}" , Name = "UpdatePartialVilla")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APIResponse>> UpdatePartialVilla(int id, [FromBody] JsonPatchDocument<VillaUpdateDTO> villa)
    {
        if (villa == null || id == 0)
        {
            return BadRequest();
        }

        Villa? villaFromDb = await _unitOfServices.Villas.GetVilla(id, false);
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
        await _unitOfServices.Villas.UpdateVilla(villaFromDb);
        await _unitOfServices.SaveChangesAsync();
        
        return Ok(villaFromDb);
    }

    [HttpDelete("{id:int}" , Name = "DeleteVilla")]
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

            Villa? villa = await _unitOfServices.Villas.GetVilla(id);
            if (villa == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("No Villa found with the specified ID");
                return NotFound(_response);
            }

            await _unitOfServices.Villas.DeleteVilla(villa);
            await _unitOfServices.SaveChangesAsync();
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
