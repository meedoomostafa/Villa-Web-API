using AppModels.Models;
using AppModels.Models.DTOs.CompanyDTOs;

namespace AppRepository.Repository.Interfaces;

public interface ICompanyRepository : IRepository<Company> 
{
    Task UpdateAsync(Company entity);
    Task<CompanyDashboardDTO?> GetDashboardAsync(int companyUserId, CancellationToken ct = default);
}