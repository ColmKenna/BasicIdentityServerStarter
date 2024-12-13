using IdentityDataModels;
using IdentityServer.EF.DataAccess;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServerAspNetIdentity.Pages.Admin.Clients;

public class Details : PageModel
{
    private IClientsRepository _clientsRepository;

    public Details(IClientsRepository clientsRepository)
    {
        _clientsRepository = clientsRepository;
    }
    
    public ClientViewModel Client { get; private set; }
    
    public async Task OnGetAsync(string clientId)
    {
        Client = await _clientsRepository.GetClient(clientId);
    }
    
}