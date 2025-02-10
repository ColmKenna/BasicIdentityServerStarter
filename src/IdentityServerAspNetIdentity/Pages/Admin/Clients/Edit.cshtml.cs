using System.Text.Json;
using IdentityDataModels;
using IdentityServer.EF.DataAccess;
using IdentityServerAspNetIdentity.ViewModels;
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


    [BindProperty(SupportsGet = true)] public ClientViewModel Client { get; set; }


    public List<string> AvailableScopes { get; private set; }


    [BindProperty] public List<string> DeletedRedirectUris { get; set; }


    [BindProperty] public List<string> DeletedPostLogoutRedirectUris { get; set; }


    [BindProperty] public List<string> DeletedAllowedCorsOrigins { get; set; }


    public Dictionary<string, string> GrantTypes { get; set; }


    public List<string> AppTypes { get; set; }

    public ApplicationGrantInfo ApplicationGrantInfo { get; set; }


    private ApplicationGrantInfo GetApplicationGrantInfo()
    {
        var applicationGrantInfo = new ApplicationGrantInfo();

        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "GrantTypesInfo.json");
        if (System.IO.File.Exists(filePath))
        {
            var jsonData = System.IO.File.ReadAllText(filePath);
            applicationGrantInfo = JsonSerializer.Deserialize<ApplicationGrantInfo>(jsonData);
        }

        return applicationGrantInfo;
    }

    public async Task OnGetAsync(string clientId)
    {
        if (string.IsNullOrEmpty(clientId))
        {
            Client = new ClientViewModel();
        }
        else
        {
            Client = await _clientsRepository.GetClient(clientId);
        }

        AvailableScopes = await _scopesRepository.GetScopes();
        ApplicationGrantInfo = GetApplicationGrantInfo();
        GrantTypes = ApplicationGrantInfo.Applications.ToDictionary(x => x.Type, x => x.Description);
        AppTypes = GrantTypes.Keys.ToList();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            AvailableScopes = await _scopesRepository.GetScopes();
            return Page();
        }

        foreach (var deletedRedirectUri in DeletedRedirectUris)
        {
            Client.RedirectUris.Remove(deletedRedirectUri);
        }

        foreach (var deletedPostLogoutRedirectUri in DeletedPostLogoutRedirectUris)
        {
            Client.PostLogoutRedirectUris.Remove(deletedPostLogoutRedirectUri);
        }

        foreach (var deletedAllowedCorsOrigin in DeletedAllowedCorsOrigins)
        {
            Client.AllowedCorsOrigins.Remove(deletedAllowedCorsOrigin);
        }

        if (Client.Id == 0)
        {
            await _clientsRepository.CreateClient(Client);
        }
        else
        {
            await _clientsRepository.UpdateClient(Client);
        }

        return RedirectToPage("Index");
    }
}