using AppRepository.Data;
using AppRepository.Repository.Interfaces;
using AppModels.Models;

namespace AppRepository.Repository;

public class CompanyRepository : Repository<Company>, ICompanyRepository
{
    private readonly ApplicationDbContext _context;
    public CompanyRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public Task UpdateAsync(Company entity)
    {
        _context.Companies.Update(entity);
        return Task.CompletedTask;
    }
}