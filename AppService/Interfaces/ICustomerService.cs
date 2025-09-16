using VillaModels.Models;

namespace AppService.Interfaces;

public interface ICustomerService
{
    Task CreateCustomerUser(Customer customer);
}