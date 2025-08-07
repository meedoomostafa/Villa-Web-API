using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VillaModels.Models;
using VillaModels.Models.DTOs.AdminManagementDTOs;
using VillaRepository.Repository.Interfaces;

namespace VillaWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompanyApiController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    protected APIResponse _response;

    public CompanyApiController(IUnitOfWork unitOfWork , IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _response = new APIResponse()
        {
            ErrorMessages = new List<string>()
        };
    }

    [HttpGet(Name = "GetAllCompanies")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIResponse>> GetAllCompanies()
    {
        try
        {
            var  companies = await _unitOfWork.Companies.GetAllAsync();
            if (companies == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("No Companies found");
                return NotFound(_response);
            }
            _response.Result = _mapper.Map<List<CompanyDTO>>(companies);
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

    [HttpGet("{id:int}", Name = "GetCompany")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APIResponse>> GetCompany(int id)
    {
        try
        {
            if (id == 0)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("Id should not be 0");
                return BadRequest(_response);
            }
            var company = await _unitOfWork.Companies.GetAsync(c => c.Id == id);
            if (company == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("No Company found");
                return NotFound(_response);
            }
            _response.Result = _mapper.Map<CompanyDTO>(company);
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

    [HttpPost(Name = "CreateCompany")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APIResponse>> CreateCompany([FromBody] CompanyCreateDTO companyToCreate)
    {
        try
        {
            if (companyToCreate == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("Company should not be null");
                return BadRequest(_response);
            }
            
            var checkCompanyNameExistence = await _unitOfWork.Companies
                .GetAsync(c => c.Name.ToLower() == companyToCreate.Name.ToLower());
            if (checkCompanyNameExistence != null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("Company name already exists");
                return BadRequest(_response);
            }
            var company = _mapper.Map<Company>(companyToCreate);
            await _unitOfWork.Companies.CreateAsync(company);
            await _unitOfWork.SaveChangesAsync();
            _response.Result = _mapper.Map<CompanyDTO>(company);
            _response.StatusCode = HttpStatusCode.Created;
            return CreatedAtAction("GetCompany", new { id = company.Id }, _response);
        }
        catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.InternalServerError;
            _response.ErrorMessages.Add(e.Message);
        }
        return StatusCode((int)_response.StatusCode, _response);
    }

    [HttpPut(Name = "UpdateCompany")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdateCompany([FromBody] CompanyUpdateDTO companyToUpdate)
    {
        if (companyToUpdate == null)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.ErrorMessages.Add("Company should not be null");
            return BadRequest(_response);
        }

        if (companyToUpdate.Id == 0)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.ErrorMessages.Add("Company id should not be 0");
            return BadRequest(_response);
        }
        var company = await _unitOfWork.Companies.GetAsync(c => c.Id == companyToUpdate.Id);
        if (company == null)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.NotFound;
            _response.ErrorMessages.Add("No Company found");
        }
        _mapper.Map(companyToUpdate, company);
        await _unitOfWork.Companies.UpdateAsync(company!);
        await _unitOfWork.SaveChangesAsync();
        _response.Result = _mapper.Map<CompanyDTO>(company);
        _response.StatusCode = HttpStatusCode.OK;
        return Ok(_response);
    }

    [HttpDelete("{id:int}", Name = "DeleteCompany")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteCompany(int id)
    {
        if (id == 0)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.ErrorMessages.Add("Company id should not be 0");
            return BadRequest(_response);
        }
        var company = await _unitOfWork.Companies.GetAsync(c => c.Id == id);
        if (company == null)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.NotFound;
            _response.ErrorMessages.Add("No Company found");
            return NotFound(_response);
        }
        await _unitOfWork.Companies.RemoveAsync(company);
        await _unitOfWork.SaveChangesAsync();
        _response.StatusCode = HttpStatusCode.NoContent;
        return NoContent();
    }
}