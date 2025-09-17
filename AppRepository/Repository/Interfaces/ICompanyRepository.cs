using AppModels.Models;

namespace AppRepository.Repository.Interfaces;

public interface ICompanyRepository : IRepository<Company> 
{
    Task UpdateAsync(Company entity);
}