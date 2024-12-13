namespace IdentityDataModels;

public class ClientViewModel
{
    public string ClientId { get; set; }
    public string ClientName { get; set; }
    public string Description { get; set; }
    public IList<string> AllowedGrantTypes { get; set; }
    public bool AllowAccessTokensViaBrowser { get; set; }
    public bool RequireConsent { get; set; }
    public bool AlwaysIncludeUserClaimsInIdToken { get; set; }
    public int AccessTokenLifetime { get; set; }
    public IList<string> AllowedScopes { get; set; }
    public int Id { get; set; }
    
    public IList<string> RedirectUris { get; set; }
    public IList<string> PostLogoutRedirectUris { get; set; }
    public IList<string> AllowedCorsOrigins { get; set; }
    public string? ClientUri { get; set; }
}