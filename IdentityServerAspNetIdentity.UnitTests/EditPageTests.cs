using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Xunit;
using IdentityServerAspNetIdentity.Pages.Admin.Clients;
using Microsoft.Extensions.DependencyInjection;
using IdentityServerAspNetIdentity.ViewModels;
using IdentityServer.EF.DataAccess;
using System.Collections.Generic;
using IdentityDataModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection.Extensions;


namespace IdentityServerAspNetIdentity.UnitTests;

public class EditPageTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public EditPageTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll<IAuthenticationSchemeProvider>();
               
                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { options.TimeProvider = TimeProvider.System; });          
                services.PostConfigure<AuthenticationOptions>(options =>
                {
                    options.DefaultAuthenticateScheme = "Test";
                    options.DefaultChallengeScheme = "Test";
                });
                
            });
        });
    }

    [Fact]
    public async Task EditPage_BasicFirstTest()
    {
        // Arrange: Mock the repository data
        var mockClientRepo = new Mock<IClientsRepository>();
        var mockScopesRepo = new Mock<IScopesRepository>();

        var testClientId = "test-client";
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
        var testScopes = new List<string> { "scope1", "scope2" };

        mockClientRepo.Setup(repo => repo.GetClient(testClientId))
                      .ReturnsAsync(testClient);

        mockScopesRepo.Setup(repo => repo.GetScopes())
                      .ReturnsAsync(testScopes);

        // Create a factory with mocked services
        var clientFactory = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddScoped(_ => mockClientRepo.Object);
                services.AddScoped(_ => mockScopesRepo.Object);
            });
        });

        var client = clientFactory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,  // Avoids following redirects to the login page,
            HandleCookies = true
        });
        
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Test");
        // Act: Make request to the Edit page
        var response = await client.GetAsync($"/Admin/Clients/Edit?clientId={testClientId}");
        response.EnsureSuccessStatusCode();

        var responseHtml = await response.Content.ReadAsStringAsync();

        // Parse the response HTML
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(responseHtml);

        // Find the input field with the client name
        var tabElement = htmlDoc.GetHtmlElement(id: "clientinformation", className: "tab");
        
        var desiredNames = new[] { "Client.ClientId", "Client.ClientName", "Client.Description", "Client.ClientUri" };


        
        Assert.NotNull(tabElement);
        foreach (var desiredName in desiredNames)
        {
            var inputElement = tabElement.GetHtmlElement(name: desiredName);
            Assert.NotNull(inputElement);
        }
    }
}