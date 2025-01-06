using Microsoft.AspNetCore.Mvc;

namespace IdentityServerAspNetIdentity.Pages.Components.ItemsView;

public class ItemsViewViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(IEnumerable<string> items)
    {
        return View(items);
    }
}