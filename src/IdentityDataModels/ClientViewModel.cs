using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IdentityDataModels;


public class ClientViewModel
{
    public IList<string> AvailableGrantTypes => new List<string>
    {
        "authorization_code", "client_credentials", "hybrid", "implicit", "password", "device_code"
    };


    public ClientViewModel()
    {
        ClientSecrets = new List<ClientSecretViewModel>();
        AllowedScopes = new List<string>();
        AllowedCorsOrigins = new List<string>();
        RedirectUris = new List<string>();
        PostLogoutRedirectUris = new List<string>();
        AllowedGrantTypes = new List<string>();
    }

    
    public string ClientId { get; set; } = "";

    [DisplayName("Client Name")]
    [Required]
    [MinLength(3)]
    [MaxLength(100)]
    public string? ClientName { get; set; } = "";
    
    public string? Description { get; set; } = "";
    public IList<string> AllowedGrantTypes { get; set; } = new List<string>();
    [DisplayName("Require PKCE")]
    public bool RequirePkce { get; set; }
    public bool AllowAccessTokensViaBrowser { get; set; }
    public bool RequireConsent { get; set; }
    public bool AlwaysIncludeUserClaimsInIdToken { get; set; } = false;
    
    [Range(0,36000)]
     public int AccessTokenLifetime { get; set; }
    public IList<string> AllowedScopes { get; set; } = new List<string>();
    public int Id { get; set; }
    
    public IList<string> RedirectUris { get; set; } = new List<string>();
    public IList<string> PostLogoutRedirectUris { get; set; } = new List<string>();
    public IList<string> AllowedCorsOrigins { get; set; } = new List<string>();
    public string? ClientUri { get; set; }
    public List<ClientSecretViewModel> ClientSecrets { get; set; } = new List<ClientSecretViewModel>();
    public IEnumerable<string> AppTypes => 
        new List<string> { "Web", "SPA", "Native/Desktop", "Machine", "Hybrid" };
}

public class ClientSecretViewModel
{
    // should hold description and secret
    public string? Description { get; set; } = "";
    public string Secret { get; set; } = "";
}
