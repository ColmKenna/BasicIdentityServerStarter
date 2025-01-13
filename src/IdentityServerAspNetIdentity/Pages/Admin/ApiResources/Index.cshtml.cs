using IdentityServer.EF.DataAccess;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityServerAspNetIdentity.Pages.Admin.ApiResources
{
    public class Index : PageModel
    {
        private readonly IApiResourcesRepository _apiResourcesRepository;

        public Index(IApiResourcesRepository apiResourcesRepository)
        {
            _apiResourcesRepository = apiResourcesRepository;
        }

        public IList<ApiResource> ApiResources { get; private set; }

        public async Task OnGetAsync()
        {
            ApiResources = await _apiResourcesRepository.GetApiResources();
        }
    }
}
