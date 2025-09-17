using AppModels.Models;

namespace AppService.Interfaces;

public interface ICompanyService 
{
    Task<Company> CheckCompanyExistence(int companyId);
    Task CreateCompanyUser(Company company);
}