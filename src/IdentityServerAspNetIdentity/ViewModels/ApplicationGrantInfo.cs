namespace IdentityServerAspNetIdentity.ViewModels;

public class GrantInfo
{
    public string Type { get; set; }
    public string GrantType { get; set; }
    public string PKCE { get; set; }
    public string Description { get; set; }
}