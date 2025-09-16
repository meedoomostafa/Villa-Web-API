using AppRepository.Data;
using AppRepository.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using VillaModels.Models;

namespace AppRepository.Repository;

public class BookingRepository : Repository<Booking> , IBookingRepository
{
    private readonly ApplicationDbContext _context;
    public BookingRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public Task UpdateAsync(Booking entity)
    {
        _context.Bookings.Update(entity);
        return Task.CompletedTask;
    }

    public async Task DeleteAllAsync()
    {
        await _context.Bookings.ExecuteDeleteAsync();
    }
}