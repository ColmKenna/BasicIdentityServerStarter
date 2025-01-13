using IdentityServer.EF.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace IdentityServerAspNetIdentity.Pages.Admin.ApiResources
{
    public class Create : PageModel
    {
        private readonly IApiResourcesRepository _apiResourcesRepository;

        public Create(IApiResourcesRepository apiResourcesRepository)
        {
            _apiResourcesRepository = apiResourcesRepository;
        }

        [BindProperty]
        public string Name { get; set; }

        [BindProperty]
        public string DisplayName { get; set; }

        [BindProperty]
        public string Description { get; set; }

        [BindProperty]
        public bool Enabled { get; set; }

        [BindProperty]
        public bool ShowInDiscoveryDocument { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var apiResource = new ApiResource
            {
                Name = Name,
                DisplayName = DisplayName,
                Description = Description,
                Enabled = Enabled,
                ShowInDiscoveryDocument = ShowInDiscoveryDocument
            };

            await _apiResourcesRepository.CreateApiResource(apiResource);

            return RedirectToPage("Index");
        }
    }
}
