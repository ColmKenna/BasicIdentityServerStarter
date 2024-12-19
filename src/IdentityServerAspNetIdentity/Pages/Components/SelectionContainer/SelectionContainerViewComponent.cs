using Microsoft.AspNetCore.Mvc;

namespace IdentityServerAspNetIdentity.Pages.Components.SelectionContainer;

public class SelectionContainerViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(string id, IEnumerable<string> source, IEnumerable<string> selected, string name,string cssClass="",  string title = "", bool allowAdd = false)
    {
        var model = new SelectionContainerViewModel
        {
            Id = id,
            Source = source,
            Selected = selected,
            Name = name,
            Title = title,
            AllowAdd = allowAdd,
            CssClass = cssClass
        };

        return View(model);
    }
}