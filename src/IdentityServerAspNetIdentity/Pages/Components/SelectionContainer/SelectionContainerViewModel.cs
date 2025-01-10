using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityServerAspNetIdentity.Pages.Components.SelectionContainer;

public class SelectionContainerViewModel
{
    
    public string Id { get; set; }
    
    public IEnumerable<SelectionContainerItemModel> Source { get; set; }

    public IEnumerable<SelectionContainerItemModel> Selected { get; set; }

    public string Name { get; set; }

    public bool AllowAdd { get; set; }
    
    public string Title { get; set; } = String.Empty;
    public string CssClass { get; set; } = String.Empty;
    public bool AsRadio { get; set; }
    public Dictionary<string,string> SourceDetails { get; set; }
    public List<SelectListItem> AddFromDatasource { get; set; }
}