using IdentityDataModels;
using IdentityServer.EF.DataAccess;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServerAspNetIdentity.Pages.Admin.Accounts;

public class Details : PageModel
{
    IUserRepository userRepository;

    public Details(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }
    
    public ApplicationUserDetails ApplicationUserDetails { get; set; }
    public async Task OnGetAsync(string id)
    {
        ApplicationUserDetails = await userRepository.GetUserById(id);
    }
}