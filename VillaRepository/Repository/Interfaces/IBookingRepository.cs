using VillaModels.Models;

namespace VillaRepository.Repository.Interfaces;

public interface IBookingRepository : IRepository<Booking>
{
    Task UpdateAsync(Booking entity);
}