using IdentityServer.EF.DataAccess;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServerAspNetIdentity.Pages.Admin.Roles;

public class Index : PageModel
{
    
    IRolesRepository rolesRepository;

    public Index(IRolesRepository rolesRepository)
    {
        this.rolesRepository = rolesRepository;
    }
    
    
    public List<Role> Roles { get; private set; }

    public async Task OnGetAsync()
    {
        Roles = await rolesRepository.GetRoles();
    }

}