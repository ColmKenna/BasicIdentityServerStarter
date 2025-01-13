using IdentityDataModels;
using IdentityServer.EF.DataAccess;
using Microsoft.AspNetCore.Mvc;
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
    
    [BindProperty]
    public Role Role { get;  set; }

    [BindProperty]
    public List<ApplicationUserSummary> Users { get; private set; }
    public List<ApplicationUserSummary> AvailableUsers { get; set; }
    
    public List<SelectListItem> UserSelectListItems => AvailableUsers.Select(u => new SelectListItem(u.UserName, u.Id)).ToList();

    public bool IsNewRole => string.IsNullOrEmpty(Role.Id);
    
    public async Task OnGetAsync(string roleName)
    {
        if( roleName == null)
        {
            Role = new Role();
            Role.Id = "";
            Role.Name = "";
            
            AvailableUsers = await userRepository.GetUsers();
            Users = new List<ApplicationUserSummary>();
            return;
        }
        Role = await rolesRepository.GetRoleByName(roleName);
        Users = await rolesRepository.GetUsersInRole(roleName);
        AvailableUsers = await userRepository.GetUsers();

    }
    
    public async Task<IActionResult> OnPostAsync()
    {
        
    if (string.IsNullOrEmpty(Role.Id))
        {
            Role=            await         rolesRepository.CreateRole(Role);

        }

        // read the array Users from the form
        var selectedUsers = Request.Form["Users[]"].ToArray();
        Users = await rolesRepository.GetUsersInRole(Role.Name);
        
        // add the selected users to the role if they are not already in the role
        foreach (var userId in selectedUsers)
        {
            if (Users.All(u => u.Id != userId))
            {
                await userRepository.AddUserToRole(userId, Role.Name);
            }
        }
        
        // remove the users that were not selected from the role
        foreach (var user in Users)
        {
            if (!selectedUsers.Contains(user.Id))
            {
                await userRepository.RemoveUserFromRole(user.Id, Role.Name);
            }
        }
        
        return RedirectToPage("Details",  new {roleName = Role.Name} );
    }

}