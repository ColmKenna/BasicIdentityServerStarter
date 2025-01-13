using IdentityServer.EF.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace IdentityServerAspNetIdentity.Pages.Admin.ApiResources
{
    public class Edit : PageModel
    {
        private readonly IApiResourcesRepository _apiResourcesRepository;

        public Edit(IApiResourcesRepository apiResourcesRepository)
        {
            _apiResourcesRepository = apiResourcesRepository;
        }

        [BindProperty]
        public ApiResource ApiResource { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            ApiResource = await _apiResourcesRepository.GetApiResourceById(id);
            if (ApiResource == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var updated = await _apiResourcesRepository.UpdateApiResource(ApiResource);
            if (!updated)
            {
                return NotFound();
            }

            return RedirectToPage("Index");
        }
    }
}
