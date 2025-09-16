using VillaModels.Models;

namespace AppService.Interfaces;

public interface IVillaService
{
    Task<List<Villa>> GetAllVillas();
    Task<Villa> GetVillaWithVillaNumbers(int id);
    Task<Villa> GetVilla(int id , bool tracked = true);
    Task<Villa> CheckVillaNameExistence(string name);
    Task CreateVilla(Villa entity);
    Task UpdateVilla(Villa entity);
    Task DeleteVilla(Villa entity);
}