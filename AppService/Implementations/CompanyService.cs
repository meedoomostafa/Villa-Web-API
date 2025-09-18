using AppService.Interfaces;
using AppModels.Models;
using AppModels.Models.DTOs.ProfilesDTOs;
using AppRepository.Repository.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AppService.Implementations;

public class CompanyService : ICompanyService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CompanyService(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Company> CheckCompanyExistence(int companyId)
    {
        return await _unitOfWork.Company.GetAsync(c => c.ApplicationUserId == companyId);
    }

    public async Task CreateCompanyUser(Company company)
    {
        await _unitOfWork.Company.CreateAsync(company);
    }

    public async Task UpdateCompanyUser(Company company)
    {
        await _unitOfWork.Company.UpdateAsync(company);
    }

    public async Task<Company> GetCompanyById(int id)
    {
        var company = await _unitOfWork.Company
            .GetAsync(c => c.ApplicationUserId == id
                ,include: q => q.Include(c => c.ApplicationUser));
        if (company == null)
            return null;
        return company;
    }
}