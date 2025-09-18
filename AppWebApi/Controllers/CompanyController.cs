using System.Net;
using AppModels.Models;
using AppModels.Models.DTOs.ProfilesDTOs;
using AppService.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AppWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CompanyController : ControllerBase
{
    private readonly IUnitOfServices _unitOfServices;
    private readonly IMapper _mapper;
    public CompanyController(
        IUnitOfServices unitOfServices
        ,IMapper mapper)
    {
        _unitOfServices = unitOfServices;
        _mapper = mapper;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<APIResponse>> CompanyProfile(int id)
    {
        try
        {
            var company = await _unitOfServices.Companies
                .GetCompanyById(id);
            if (company == null)
            {
                return NotFound(new APIResponse()
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.NotFound,
                    ErrorMessages = new List<string>(){"No Company found"}
                });
            }
            return Ok(new APIResponse()
            {
                StatusCode = HttpStatusCode.OK,
                Result = _mapper.Map<CompanyProfileDTO>(company)
            });
        }
        catch (Exception e)
        {
            return StatusCode((int)StatusCodes.Status500InternalServerError, new APIResponse()
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                ErrorMessages = new List<string>() { e.Message }
            });
        }
    }
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIResponse>> UpdateCompanyProfile([FromBody] CompanyProfileDTO companyDto)
    {
        try
        {
            if (companyDto == null || companyDto.ApplicationUserId == 0)
            {
                return BadRequest(new APIResponse()
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest
                });
            }

            var company = await _unitOfServices.Companies
                .GetCompanyById(companyDto.ApplicationUserId);
            
            _mapper.Map(companyDto, company);
            await _unitOfServices.Companies.UpdateCompanyUser(company);
            await _unitOfServices.SaveChangesAsync();
            return Ok(new APIResponse()
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.Created
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}