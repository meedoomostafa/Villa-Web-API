using AppService.Interfaces;
using VillaModels.Models;
using AppRepository.Repository.Interfaces;

namespace AppService.Implementations;

public class CustomerService : ICustomerService
{
    private readonly IUnitOfWork _unitOfWork;

    public CustomerService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task CreateCustomerUser(Customer customer)
    {
        await _unitOfWork.Customer.CreateAsync(customer);
    }
}