using AppWebApiUtilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using VillaModels.Models;

namespace AppRepository.Seeder;

public sealed class SeedAdminAccount
{
    public static async Task SeedAdminAccounts(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

        string adminEmail = "admin@gmail.com";
        string adminPassword = "Admin@123";
        
        var  adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new ApplicationUser { UserName = adminEmail, Email = adminEmail };
        }

        try
        {
            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
            {
                if (!await roleManager.RoleExistsAsync(ApplicationRoles.AdminRoleName))
                {
                    await roleManager.CreateAsync(new ApplicationRole(ApplicationRoles.AdminRoleName));
                }
            
                await userManager.AddToRoleAsync(adminUser, ApplicationRoles.AdminRoleName);
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"error: {error.Description}");
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
        
    }
}