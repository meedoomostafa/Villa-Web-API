using AppWebApiUtilities;
using Microsoft.AspNetCore.Identity;
using VillaModels.Models;

namespace AppRepository.Seeder;

public sealed class SeedApplicationRoles
{
    public static async Task SeedRoles(RoleManager<ApplicationRole> roleManager)
    {
        string[] roleNames = { ApplicationRoles.AdminRoleName , ApplicationRoles.CompanyRoleName , ApplicationRoles.CustomerRoleName };
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new ApplicationRole(roleName));
            }
        }
    }
}