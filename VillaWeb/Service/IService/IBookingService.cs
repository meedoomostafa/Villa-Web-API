using VillaWeb.Models.DTOs.BookingDTOs;
using VillaWeb.Models.Requests;
using VillaWeb.Models.ResponseTypes;

namespace VillaWeb.Service.IService;

public interface IBookingService : IBaseService
{
    Task<T> GetAllAsync<T>() where T : APIResponse, new();
    Task<T> GetAsync<T>(int id)  where T : APIResponse, new();
    Task<T> CreateAsync<T>(BookingCreateDTO dto)  where T : APIResponse, new();
    Task<T> UpdateAsync<T>(BookingUpdateDTO dto)   where T : APIResponse, new();
    Task<T> DeleteAsync<T>(int id) where T : APIResponse, new();
}