using Microsoft.AspNetCore.Identity;
using VillaModels.Models;

namespace AppService.Interfaces;

public interface IAccountService
{
    Task<ApplicationUser> GetUserByEmailAsync(string email);
    Task<ApplicationUser> GetUserByIdAsync(int id);
    Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);
    Task<IdentityResult> AssignUserToRoleAsync(ApplicationUser user, string roleName);
    Task<IList<string>> GetRolesAsync(ApplicationUser user);
}