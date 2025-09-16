using AppService.Interfaces;
using VillaModels.Models;
using AppRepository.Repository.Interfaces;

namespace AppService.Implementations;

public class CompanyService : ICompanyService
{
    private IUnitOfWork _unitOfWork;

    public CompanyService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Company> CheckCompanyExistence(int companyId)
    {
        return await _unitOfWork.Company.GetAsync(c => c.ApplicationUserId == companyId);
    }

    public async Task CreateCompanyUser(Company company)
    {
        await _unitOfWork.Company.CreateAsync(company);
    }
}