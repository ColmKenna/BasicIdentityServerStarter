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

public class ClientEditPageTabsContainCorrectInputsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;


    public ClientEditPageTabsContainCorrectInputsTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithTestAuthentication();
    }


    private HttpClient CreateHttpClient(Mock<IClientsRepository> mockClientRepo, Mock<IScopesRepository> mockScopesRepo)
    {
        var clientFactory = _factory.WithMockServices(mockClientRepo, mockScopesRepo);
        
        var client = clientFactory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
            HandleCookies = true
        });
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Test");
        return client;
    }


    [Theory]
    [ClassData(typeof(EditPageTestData))]
    public async Task EditPage_TabContainsExpectedInputs(string tabId, InputItem[] expectedInputs)
    {
        // Arrange: Mock the repository data
        var mockClientRepo = new Mock<IClientsRepository>();
        var mockScopesRepo = new Mock<IScopesRepository>();

        ClientViewModel? testClient = TestClientDataFactory.GetTestClientViewModel();
        var testClientId = testClient.ClientId;
        List<string> testScopes = new List<string> { "scope1", "scope2", "scope3" };

        mockClientRepo.Setup(m => m.GetClient(testClientId)).ReturnsAsync(testClient);
        mockScopesRepo.Setup(m => m.GetScopes()).ReturnsAsync(testScopes);

        var client = CreateHttpClient(mockClientRepo, mockScopesRepo);


        // Act: Make request to the Edit page
        var response = await client.GetAsync($"/Admin/Clients/Edit?clientId={testClientId}");
        response.EnsureSuccessStatusCode();

        var responseHtml = await response.Content.ReadAsStringAsync();

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(responseHtml);

        
        // Assert: Verify the tab contains the expected inputs
        var tabElement = htmlDoc.GetHtmlElement(id: tabId, className: "tab");
        Assert.NotNull(tabElement);
        foreach (var input in expectedInputs)
        {
            var inputElement = tabElement.GetHtmlElement(name: input.Name);
            Assert.NotNull(inputElement);
            var inputType = inputElement.Attributes["type"]?.Value;
            Assert.Equal(input.Type, inputType);
        }
    }
}