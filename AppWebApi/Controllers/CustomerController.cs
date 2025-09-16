using Microsoft.AspNetCore.Mvc;
using AppRepository.Repository.Interfaces;

namespace AppWebApi.Controllers;
[Route("/api/[controller]")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public CustomerController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
}