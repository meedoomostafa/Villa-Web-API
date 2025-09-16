using AppRepository.Data;
using AppRepository.Repository.Interfaces;
using VillaModels.Models;

namespace AppRepository.Repository;

public class CustomerRepository : Repository<Customer> , ICustomerRepository
{
    private readonly ApplicationDbContext _context;
    public CustomerRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public Task UpdateAsync(Customer entity)
    {
        _context.Customers.Update(entity);
        return Task.CompletedTask;
    }
}