using IdentityServer.EF.DataAccess;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace IdentityServerAspNetIdentity.Pages.Admin.ApiResources
{
    public class Details : PageModel
    {
        private readonly IApiResourcesRepository _apiResourcesRepository;

        public Details(IApiResourcesRepository apiResourcesRepository)
        {
            _apiResourcesRepository = apiResourcesRepository;
        }

        public ApiResource ApiResource { get; private set; }

        public async Task OnGetAsync(int id)
        {
            ApiResource = await _apiResourcesRepository.GetApiResourceById(id);
        }
    }
}
