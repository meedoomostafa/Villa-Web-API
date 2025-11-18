using AppRepository.Data;
using AppRepository.Repository.Interfaces;
using AppModels.Models;
using AppModels.Models.DTOs.CompanyDTOs;
using AppWebApiUtilities;
using Microsoft.EntityFrameworkCore;

namespace AppRepository.Repository;

public class CompanyRepository : Repository<Company>, ICompanyRepository
{
    private readonly ApplicationDbContext _context;
    public CompanyRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public Task UpdateAsync(Company entity)
    {
        _context.Companies.Update(entity);
        return Task.CompletedTask;
    }

    public async Task<CompanyDashboardDTO?> GetDashboardAsync(int companyUserId, CancellationToken ct = default)
    {
         var company = await _context.Companies
            .Where(c => c.ApplicationUserId == companyUserId)
            .Select(c => new { c.ApplicationUserId, c.CompanyName, c.Country, c.City })
            .FirstOrDefaultAsync(ct);
            
        if (company == null)
            return null;

        var villaData = await (
            from v in _context.Villas
            join vn in _context.VillaNumbers on v.Id equals vn.VillaId into vnGroup
            from vn in vnGroup.DefaultIfEmpty()
            join b in _context.Bookings on vn.Id equals b.VillaNumberId into bookingGroup
            from b in bookingGroup.DefaultIfEmpty()
            join cust in _context.Customers on b.CustomerId equals cust.ApplicationUserId into custGroup
            from cust in custGroup.DefaultIfEmpty()
            join usr in _context.Users on cust.ApplicationUserId equals usr.Id into userGroup
            from usr in userGroup.DefaultIfEmpty()
            where v.CompanyId == companyUserId
            select new
            {
                // Villa
                VillaId = v.Id,
                VillaName = v.Name,
                VillaDetails = v.Details,
                VillaPrice = v.Price,
                VillaImageUrl = v.ImageUrl,
                VillaCity = v.City,
                
                VillaNumberId = vn != null ? vn.Id : (int?)null,
                VillaNumberSpecialDetails = vn != null ? vn.SpecialDetails : null,
                
                BookingId = b != null ? b.Id : (int?)null,
                BookingStartDate = b != null ? b.StartDate : (DateTime?)null,
                BookingEndDate = b != null ? b.EndDate : (DateTime?)null,
                BookingTotalPrice = b != null ? b.TotalPrice : (decimal?)null,
                BookingStatus = b != null ? b.Status : (BookingStatus?)null,
                
                CustomerUserId = cust != null ? cust.ApplicationUserId : (int?)null,
                CustomerFullName = cust != null ? cust.FullName : null,
                CustomerEmail = usr != null ? usr.Email : null,
                CustomerUserName = usr != null ? usr.UserName : null
            }).ToListAsync(ct);

        return new CompanyDashboardDTO
        {
            CompanyId = company.ApplicationUserId,
            CompanyName = company.CompanyName,
            Country = company.Country,
            City = company.City,
            Villas = villaData
                .GroupBy(d => d.VillaId)
                .Select(villaGroup => new VillaDashboardDTO
                {
                    VillaId = villaGroup.Key,
                    Name = villaGroup.First().VillaName,
                    Details = villaGroup.First().VillaDetails,
                    Price = villaGroup.First().VillaPrice,
                    ImageUrl = villaGroup.First().VillaImageUrl,
                    City = villaGroup.First().VillaCity,
                    VillaNumbers = villaGroup
                        .Where(d => d.VillaNumberId.HasValue)
                        .GroupBy(d => d.VillaNumberId)
                        .Select(vnGroup => new VillaNumberDashboardDTO
                        {
                            VillaNumberId = vnGroup.Key.Value,
                            SpecialDetails = vnGroup.First().VillaNumberSpecialDetails,
                            Bookings = vnGroup
                                .Where(d => d.BookingId.HasValue)
                                .Select(d => new BookingDashboardDTO
                                {
                                    BookingId = d.BookingId.Value,
                                    StartDate = d.BookingStartDate.Value,
                                    EndDate = d.BookingEndDate.Value,
                                    TotalPrice = d.BookingTotalPrice.Value,
                                    Status = d.BookingStatus.Value,
                                    Customer = new CustomerDashboardDTO
                                    {
                                        ApplicationUserId = d.CustomerUserId.Value,
                                        FullName = d.CustomerFullName,
                                        Email = d.CustomerEmail,
                                        UserName = d.CustomerUserName
                                    }
                                }).ToList()
                        }).ToList()
                }).ToList()
        };
    }
}