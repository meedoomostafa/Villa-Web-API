using AppRepository.Data;
using AppRepository.Repository.Interfaces;
using VillaModels.Models;

namespace AppRepository.Repository;

public class ReviewRepository : Repository<Review> , IReviewRepository
{
    private readonly ApplicationDbContext _context;
    public ReviewRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public Task UpdateAsync(Review entity)
    {
        _context.Reviews.Update(entity);
        return Task.CompletedTask;
    }
}