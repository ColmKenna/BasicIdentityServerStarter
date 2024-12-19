namespace IdentityServerAspNetIdentity.Pages.Components.StringArrayContainer;

public class StringArrayContainerViewModel  
{
    public string Id { get; set; }
    
    public IEnumerable<string> Current { get; set; }

    public string Name { get; set; }

    public bool AllowAdd { get; set; }
    
    public string Title { get; set; } = String.Empty;
    public string CssClass { get; set; } = String.Empty;
}