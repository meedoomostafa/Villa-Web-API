using AppModels.Models;
using AppRepository.Repository.Interfaces;
using AppService.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AppService.Implementations;

public class VillaService : IVillaService
{
    private readonly IUnitOfWork _unitOfWork;

    public VillaService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<List<Villa>> GetAllVillas()
    {
        return await _unitOfWork.Villa.GetAllAsync();
    }

    public async Task<List<Villa>> GetAllCompanyVillas(int id)
    {
        return await _unitOfWork.Villa.GetAllAsync(v => v.CompanyId == id);
    }

    public async Task<Villa> GetVillaWithVillaNumbers(int id)
    {
        return await _unitOfWork.Villa
            .GetAsync(v => v.Id == id, include: q => q.Include(v => v.VillaNumbers));
    }

    public async Task<Villa> GetVilla(int id ,  bool tracked = true)
    {
        return await _unitOfWork.Villa.GetAsync(v => v.Id == id , tracked: tracked);
    }

    public async Task<Villa> CheckVillaNameExistence(string name)
    {
        return await _unitOfWork.Villa
            .GetAsync(u => u.Name.ToLower() == name.ToLower());
    }

    public async Task CreateVilla(Villa entity)
    {
        await _unitOfWork.Villa.CreateAsync(entity);
    }

    public async Task UpdateVilla(Villa entity)
    {
        await _unitOfWork.Villa.UpdateAsync(entity);
    }

    public async Task DeleteVilla(Villa entity)
    {
        await _unitOfWork.Villa.RemoveAsync(entity);
    }
}