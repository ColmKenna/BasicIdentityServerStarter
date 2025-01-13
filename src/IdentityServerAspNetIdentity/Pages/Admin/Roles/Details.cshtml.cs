using IdentityDataModels;
using IdentityServer.EF.DataAccess;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServerAspNetIdentity.Pages.Admin.Roles;

public class Details : PageModel
{
    
    IRolesRepository rolesRepository;

    public Details(IRolesRepository rolesRepository)
    {
        this.rolesRepository = rolesRepository;
    }
    
    
    public Role Role { get; private set; }
    public List<ApplicationUserSummary> Users { get; private set; }
    
    
    public async Task OnGetAsync(string roleName)
    {
        Role = await rolesRepository.GetRoleByName(roleName);
        Users = await rolesRepository.GetUsersInRole(roleName);
        
        
    }
    
}