﻿namespace IdentityDataModels;


public class ClientViewModel
{
    public IList<string> AvailableGrantTypes => new List<string>
    {
        "authorization_code", "client_credentials", "hybrid", "implicit", "password", "device_code"
    };
    

    
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
    public List<ClientSecretViewModel> ClientSecrets { get; set; } = new List<ClientSecretViewModel>();
}

public class ClientSecretViewModel
{
    // should hold description and secret
    public string Description { get; set; }
    public string Secret { get; set; }
}