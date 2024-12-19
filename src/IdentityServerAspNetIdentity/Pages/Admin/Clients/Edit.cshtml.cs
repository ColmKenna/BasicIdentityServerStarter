using IdentityDataModels;
using IdentityServer.EF.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServerAspNetIdentity.Pages.Admin.Clients;

public class Edit : PageModel
{
    private IClientsRepository _clientsRepository;
    IScopesRepository _scopesRepository;

    public Edit(IClientsRepository clientsRepository, IScopesRepository scopesRepository)
    {
        _clientsRepository = clientsRepository;
        _scopesRepository = scopesRepository;
    }
    
    [BindProperty(SupportsGet = true)]
    public ClientViewModel Client { get;  set; }
    public List<string> AvailableScopes { get; private set; }
    
    public async Task OnGetAsync(string clientId)
    {
        Client = await _clientsRepository.GetClient(clientId);
        AvailableScopes = await _scopesRepository.GetScopes();
    }
    
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            AvailableScopes = await _scopesRepository.GetScopes();
            return Page();
        }

        var result = await _clientsRepository.UpdateClient(Client);
        return RedirectToPage("Index");
    }

}