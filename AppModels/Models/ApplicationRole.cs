using Microsoft.AspNetCore.Identity;

namespace AppModels.Models;

public class ApplicationRole : IdentityRole<int>
{
    public ApplicationRole(string name) : base(name)
    {
        
    }
}