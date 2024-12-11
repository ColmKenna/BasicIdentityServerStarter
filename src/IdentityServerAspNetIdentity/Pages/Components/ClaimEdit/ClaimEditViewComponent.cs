using IdentityDataModels;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServerAspNetIdentity.Pages.Components.ClaimEdit;

public class ClaimEditViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(UserClaimSummary claim, int index)
    {
        return View((claim, index));
    }
}


