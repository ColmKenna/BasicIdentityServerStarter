using Microsoft.AspNetCore.Mvc;

namespace IdentityServerAspNetIdentity.Pages.Components.HtmlModal;

public class HtmlModalViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(ModalViewModel model)
    {
        return View(model);
    }
}