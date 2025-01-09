using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityServerAspNetIdentity.Pages.Components.SelectionContainer;

public class SelectionContainerItemModel
{
    protected bool Equals(SelectionContainerItemModel other)
    {
        return Id == other.Id && Value == other.Value;
    }

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((SelectionContainerItemModel)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Value);
    }

    public string Id { get; set; }
    public string Value { get; set; }
}
public class SelectionContainerViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(string id, IEnumerable<SelectionContainerItemModel> source, string name,IEnumerable<SelectionContainerItemModel> selected = null, string cssClass="",  string title = "", bool allowAdd = false, List<SelectListItem> addFromDatasource = null,  bool asRadio = false,Dictionary<string, string> sourceDetails = null)
    {
        
        sourceDetails ??= new Dictionary<string, string>();
        selected ??= new List<SelectionContainerItemModel>();
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