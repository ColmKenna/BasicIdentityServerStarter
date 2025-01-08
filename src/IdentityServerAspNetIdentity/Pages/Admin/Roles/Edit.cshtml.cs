using IdentityDataModels;
using IdentityServer.EF.DataAccess;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityServerAspNetIdentity.Pages.Admin.Roles;

public class Edit : PageModel
{
    IRolesRepository rolesRepository;
    private IUserRepository userRepository;
    
    public Edit(IRolesRepository rolesRepository, IUserRepository userRepository)
    {
        this.rolesRepository = rolesRepository;
        this.userRepository = userRepository;
    }
    
    public Role Role { get; private set; }
    public List<ApplicationUserSummary> Users { get; private set; }
    public List<ApplicationUserSummary> AvailableUsers { get; set; }
    
    public List<SelectListItem> UserSelectListItems => AvailableUsers.Select(u => new SelectListItem(u.UserName, u.Id)).ToList();

    
    public async Task OnGetAsync(string roleName)
    {
        Role = await rolesRepository.GetRoleByName(roleName);
        Users = await rolesRepository.GetUsersInRole(roleName);
        AvailableUsers = await userRepository.GetUsers();

    }

}