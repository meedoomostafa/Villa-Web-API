using AppModels.Models;

namespace AppRepository.Repository.Interfaces;

public interface ICustomerRepository : IRepository<Customer>
{
    Task UpdateAsync(Customer entity);
}