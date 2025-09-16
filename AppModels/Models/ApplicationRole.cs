using Microsoft.AspNetCore.Identity;

namespace VillaModels.Models;

public class ApplicationRole : IdentityRole<int>
{
    public ApplicationRole(string name) : base(name)
    {
        
    }
}