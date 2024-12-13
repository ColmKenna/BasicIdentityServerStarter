using IdentityDataModels;
using IdentityServer.EF.DataAccess;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServerAspNetIdentity.Pages.Admin.Clients;

public class Index : PageModel
{
    private IClientsRepository _clientsRepository;

    public Index(IClientsRepository clientsRepository)
    {
        _clientsRepository = clientsRepository;
    }

    public IList<ClientsSummary> Clients { get; private set; }
    
    public async Task OnGetAsync()
    {
        Clients = await _clientsRepository.GetClients();
    }
}