using VillaWeb.Models.DTOs.ProfilesDTOs;
using VillaWeb.Models.ResponseTypes;

namespace VillaWeb.Service.IService
{
    public interface ICompanyService
    {
        Task<T> GetCompanyAsync<T>(int companyId) where T : APIResponse, new();
        Task<T> UpdateCompanyAsync<T>(CompanyProfileDTO dto) where T : APIResponse, new();
    }
}
