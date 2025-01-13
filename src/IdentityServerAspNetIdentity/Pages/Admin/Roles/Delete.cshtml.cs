using IdentityDataModels;
using IdentityServer.EF.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServerAspNetIdentity.Pages.Admin.Roles;

public class Delete : PageModel
{
    
    IRolesRepository rolesRepository;

    public Delete(IRolesRepository rolesRepository)
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
    
    public async Task<IActionResult> OnPostAsync(string confirmdelete, string roleName)
    {
        string expectedConfirmation = $"I want to delete {roleName}";

        if (confirmdelete?.Trim() == expectedConfirmation)
        {
            var result = await rolesRepository.DeleteRoleByName(roleName);
            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Failed to delete the role.");
                await OnGetAsync(roleName); // Reload the current role data
                return Page();
            }
            return RedirectToPage("Index"); // Redirect to the index or another page after deletion
        }

        ModelState.AddModelError(string.Empty, $"You must type '{expectedConfirmation}' to confirm.");
        await OnGetAsync(roleName); // Reload the current role data
        return Page();
    }
    
}