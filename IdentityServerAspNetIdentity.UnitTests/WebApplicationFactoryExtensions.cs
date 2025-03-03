using IdentityServer.EF.DataAccess;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace IdentityServerAspNetIdentity.UnitTests;

public static class WebApplicationFactoryExtensions
{
    public static WebApplicationFactory<Program> WithTestAuthentication(this WebApplicationFactory<Program> factory)
    {
        return factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                AuthenticationServiceCollectionExtensions.AddAuthentication(services, "Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options =>
                    {
                        options.TimeProvider = TimeProvider.System;
                    });

                OptionsServiceCollectionExtensions.PostConfigure<AuthenticationOptions>(services, options =>
                {
                    options.DefaultAuthenticateScheme = "Test";
                    options.DefaultChallengeScheme = "Test";
                });
            });
        });
    }

    public static WebApplicationFactory<Program> WithMockServices<T1,T2>(
        this WebApplicationFactory<Program> factory,
        Mock<T1> mockT1,
        Mock<T2> mockT2) where T1 : class where T2 : class
    {
        return factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddScoped(_ => mockT1.Object);
                services.AddScoped(_ => mockT2.Object);
            });
        });
    }
}