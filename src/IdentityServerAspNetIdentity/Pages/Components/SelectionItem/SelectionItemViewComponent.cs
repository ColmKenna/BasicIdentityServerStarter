using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace IdentityServerAspNetIdentity.Pages.Components.SelectionItem;

public class SelectionItemViewComponent : ViewComponent
{
    
    // propertoes for name ischecked value
    
    public string Name { get; set; }
    public bool IsChecked { get; set; }
    public string Value { get; set; }
    public bool AsRadio { get; set; }
    
    public string Description { get; set; }
    public string ParentId { get; set; }

    public IViewComponentResult Invoke(string name, bool isChecked, string value, string parentId = "",    bool asRadio = false, string description = "")
    {
        Name = name;
        IsChecked = isChecked;
        Value = value;
        AsRadio = asRadio;
        Description = description;
        ParentId = parentId;
        
        return View(this);
    }
  
}