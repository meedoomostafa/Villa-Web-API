using VillaModels.Models;

namespace AppRepository.Repository.Interfaces;

public interface IBookingRepository : IRepository<Booking>
{
    Task UpdateAsync(Booking entity);
    Task DeleteAllAsync();
}