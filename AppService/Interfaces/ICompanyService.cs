using AppModels.Models;
using AppModels.Models.DTOs.ProfilesDTOs;

namespace AppService.Interfaces;

public interface ICompanyService 
{
    Task<Company> CheckCompanyExistence(int companyId);
    Task CreateCompanyUser(Company company);
    Task UpdateCompanyUser(Company company);
    Task<Company> GetCompanyById(int id);
}