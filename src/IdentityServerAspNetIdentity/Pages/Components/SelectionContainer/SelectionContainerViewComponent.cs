using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityServerAspNetIdentity.Pages.Components.SelectionContainer;

public class SelectionContainerViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(string id, IEnumerable<string> source, string name,IEnumerable<string> selected = null, string cssClass="",  string title = "", bool allowAdd = false, List<SelectListItem> addFromDatasource = null,  bool asRadio = false,Dictionary<string, string> sourceDetails = null)
    {
        
        sourceDetails ??= new Dictionary<string, string>();
        selected ??= new List<string>();
        var model = new SelectionContainerViewModel
        {
            Id = id,
            Source = source,
            Selected = selected,
            Name = name,
            Title = title,
            AllowAdd = allowAdd,
            CssClass = cssClass,
            AsRadio = asRadio,
            SourceDetails = sourceDetails,
            AddFromDatasource = addFromDatasource
        };

        return View(model);
    }
}