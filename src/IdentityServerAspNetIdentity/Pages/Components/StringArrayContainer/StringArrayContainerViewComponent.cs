using Microsoft.AspNetCore.Mvc;

namespace IdentityServerAspNetIdentity.Pages.Components.StringArrayContainer;

public class StringArrayContainerViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(string id, IEnumerable<string> current, string name, string cssClass="", string title="", bool allowAdd= false)
    {
        var model = new StringArrayContainerViewModel
        {
            Id = id,
            Current = current,
            Name = name,
            Title = title,
            AllowAdd = allowAdd,
            CssClass = cssClass
        };
        return View(model);
    }
    
}