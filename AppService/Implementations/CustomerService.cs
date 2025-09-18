using AppService.Interfaces;
using AppModels.Models;
using AppModels.Models.DTOs.ProfilesDTOs;
using AppRepository.Repository.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AppService.Implementations;

public class CustomerService : ICustomerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CustomerService(
        IUnitOfWork unitOfWork
        ,IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task CreateCustomerUser(Customer customer)
    {
        await _unitOfWork.Customer.CreateAsync(customer);
    }

    public async Task UpdateCustomerUser(Customer customer)
    {
        await _unitOfWork.Customer.UpdateAsync(customer);
    }

    public async Task<Customer> GetCustomerById(int id)
    {
        var customer = await _unitOfWork.Customer
            .GetAsync(c => c.ApplicationUserId == id
                ,include: q => q.Include(c => c.ApplicationUser));
        
        if (customer == null)
            return null;

        return customer;
    }
}