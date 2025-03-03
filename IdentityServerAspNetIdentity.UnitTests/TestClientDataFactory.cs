using IdentityDataModels;

namespace IdentityServerAspNetIdentity.UnitTests;

public static class TestClientDataFactory
{
    public static ClientViewModel GetTestClientViewModel()
    {
        var testClient = new ClientViewModel
        {
            Id = 1,
            ClientName = "Test Client",
            ClientId = "test-client-id",
            Description = "Test Description",
            ClientUri = "http://test-client-uri",
            AllowedGrantTypes = new List<string> { "authorization_code", "client_credentials" },
            RequirePkce = true,
            AllowAccessTokensViaBrowser = true,
            RequireConsent = true,
            AlwaysIncludeUserClaimsInIdToken = true,
            AccessTokenLifetime = 3600,
            AllowedScopes = new List<string> { "scope1", "scope2" },
            RedirectUris = new List<string> { "http://redirect-uri1", "http://redirect-uri2" },
            PostLogoutRedirectUris = new List<string> { "http://post-logout-uri1", "http://post-logout-uri2" },
            AllowedCorsOrigins = new List<string> { "http://cors-origin1", "http://cors-origin2" },
            ClientSecrets = new List<ClientSecretViewModel>
            {
                new ClientSecretViewModel { Description = "Secret1", Secret = "secret1" },
                new ClientSecretViewModel { Description = "Secret2", Secret = "secret2" }
            }
        };
        return testClient;
    }
}