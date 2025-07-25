using VillaModels.Models;
using VillaRepository.Data;
using VillaRepository.Repository.Interfaces;

namespace VillaRepository.Repository;

public class BookingRepository : Repository<Booking> , IBookingRepository
{
    private readonly ApplicationDbContext _context;
    public BookingRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task UpdateAsync(Booking entity)
    {
        _context.Bookings.Update(entity);
    }
}