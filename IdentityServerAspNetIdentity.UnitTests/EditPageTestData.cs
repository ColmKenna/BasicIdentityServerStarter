namespace IdentityServerAspNetIdentity.UnitTests;


public class EditPageTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[]
        {
            "clientinformation", new[]
            {
                new InputItem("Client.ClientId", "hidden"),
                new InputItem("Client.ClientName", "text"),
                new InputItem("Client.Description", "text"),
                new InputItem("Client.ClientUri", "text")
            }
        };
        yield return new object[]
        {
            "security", new[]
            {
                new InputItem("Client.ClientSecrets[0].Description", "text"),
                new InputItem("Client.ClientSecrets[0].Secret", "hidden"),
                new InputItem("Client.RequireConsent", "checkbox"),
                new InputItem("Client.AllowAccessTokensViaBrowser", "checkbox"),
                new InputItem("Client.AccessTokenLifetime", "number")
            }
        };
        yield return new object[]
        {
            "granttypes", new[]
            {
                new InputItem("Client.RequirePkce", "checkbox"),
                new InputItem("Client.AllowedGrantTypes[]", "checkbox")
            }
        };
        yield return new object[]
        {
            "scopes", new[]
            {
                new InputItem("Client.AllowedScopes[]", "checkbox")
            }
        };
        yield return new object[]
        {
            "uris", new[]
            {
                new InputItem("Client.RedirectUris[0]", "text"),
                new InputItem("Client.RedirectUris[1]", "text"),
                new InputItem("Client.PostLogoutRedirectUris[0]", "text"),
                new InputItem("Client.PostLogoutRedirectUris[1]", "text"),
                new InputItem("Client.AllowedCorsOrigins[0]", "text"),
                new InputItem("Client.AllowedCorsOrigins[1]", "text")
            }
        };
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
}
public record InputItem(string Name, string Type);
