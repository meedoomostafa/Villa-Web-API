using AppModels.Models;
using AppModels.Models.DTOs.ProfilesDTOs;

namespace AppService.Interfaces;

public interface ICustomerService
{
    Task CreateCustomerUser(Customer customer);
    Task UpdateCustomerUser(Customer customer);
    Task<Customer> GetCustomerById(int id);
}