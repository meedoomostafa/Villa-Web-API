using AppModels.Models;

namespace AppRepository.Repository.Interfaces;

public interface IReviewRepository : IRepository<Review> 
{
    Task UpdateAsync(Review entity);
}