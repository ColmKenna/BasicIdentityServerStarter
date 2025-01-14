using System.Linq.Expressions;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.EntityFramework.Entities;
using IdentityDataModels;
using Microsoft.EntityFrameworkCore;
using Client = Duende.IdentityServer.EntityFramework.Entities.Client;

namespace IdentityServer.EF.DataAccess;

public class ClientsRepository : IClientsRepository
{
    private readonly ConfigurationDbContext _dbContext;



    public Task<List<ClientsSummary>> GetClients()
    {
        return _dbContext.Clients.Select(c => new ClientsSummary
        {
            Id = c.Id,
            ClientId = c.ClientId,
            ClientName = c.ClientName,
            Description = c.Description,
            RedirectUris = c.RedirectUris.Select(r => r.RedirectUri).ToList()
        }).ToListAsync();
    }
    
    private Expression<Func<Duende.IdentityServer.EntityFramework.Entities.Client, ClientViewModel>> clientToClientViewModel = 
     c => new ClientViewModel
    {
        ClientId = c.ClientId,
        ClientName = c.ClientName,
        Description = c.Description,
        AllowedGrantTypes = c.AllowedGrantTypes.Select(g => g.GrantType).ToList(),
        RequirePkce = c.RequirePkce,
        AllowAccessTokensViaBrowser = c.AllowAccessTokensViaBrowser,
        RequireConsent = c.RequireConsent,
        AlwaysIncludeUserClaimsInIdToken = c.AlwaysIncludeUserClaimsInIdToken,
        AccessTokenLifetime = c.AccessTokenLifetime,
        AllowedScopes = c.AllowedScopes.Select(s => s.Scope).ToList(),
        Id = c.Id,
        RedirectUris = c.RedirectUris.Select(r => r.RedirectUri).ToList(),
        PostLogoutRedirectUris = c.PostLogoutRedirectUris.Select(p => p.PostLogoutRedirectUri).ToList(),   
        AllowedCorsOrigins = c.AllowedCorsOrigins.Select(o => o.Origin).ToList(),
        ClientUri = c.ClientUri,
        ClientSecrets = c.ClientSecrets.Select(s => new ClientSecretViewModel
         {
             Description = s.Description,
             Secret = s.Value
        }).ToList()
    };

    public ClientsRepository(ConfigurationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<ClientViewModel?> GetClient(string clientId)
    {
        return _dbContext.Clients
            .Include(c => c.AllowedGrantTypes)
            .Include(c => c.AllowedScopes)
            .Include(c => c.RedirectUris)
            .Include(c => c.PostLogoutRedirectUris)
            .Include(c => c.AllowedCorsOrigins)
            .Include(c => c.ClientSecrets)
            .Where(c => c.ClientId == clientId)
            .Select(clientToClientViewModel)
            .FirstOrDefaultAsync();
    }

    public Task<ClientViewModel> UpdateClient(ClientViewModel client)
    {
        var existingClient = _dbContext.Clients
            .Include(c => c.AllowedGrantTypes)
            .Include(c => c.AllowedScopes)
            .Include(c => c.RedirectUris)
            .Include(c => c.PostLogoutRedirectUris)
            .Include(c => c.AllowedCorsOrigins)
            .Include(c => c.ClientSecrets)
            .FirstOrDefault(c => c.Id == client.Id);

        if (existingClient == null)
        {
            throw new InvalidOperationException("Client not found");
        }
        
        existingClient.ClientId = client.ClientId;
        existingClient.ClientName = client.ClientName;
        existingClient.Description = client.Description;
        existingClient.AllowAccessTokensViaBrowser = client.AllowAccessTokensViaBrowser;
        existingClient.RequireConsent = client.RequireConsent;
        existingClient.AlwaysIncludeUserClaimsInIdToken = client.AlwaysIncludeUserClaimsInIdToken;
        existingClient.AccessTokenLifetime = client.AccessTokenLifetime;
        existingClient.ClientUri = client.ClientUri;
        existingClient.RedirectUris = client.RedirectUris.Select(r => new ClientRedirectUri
        {
            RedirectUri = r
        }).ToList();
        
        
        existingClient.PostLogoutRedirectUris.RemoveAll(p => !client.PostLogoutRedirectUris.Contains(p.PostLogoutRedirectUri));
        var itemsToAdd = client.PostLogoutRedirectUris.Where(p => existingClient.PostLogoutRedirectUris.All(e => e.PostLogoutRedirectUri != p));
        existingClient.PostLogoutRedirectUris.AddRange(itemsToAdd.Select(p => new ClientPostLogoutRedirectUri
        {
            PostLogoutRedirectUri = p
        }));
        
        existingClient.AllowedCorsOrigins.RemoveAll(o => !client.AllowedCorsOrigins.Contains(o.Origin));
        var originsToAdd = client.AllowedCorsOrigins.Where(o => existingClient.AllowedCorsOrigins.All(e => e.Origin != o));
        existingClient.AllowedCorsOrigins.AddRange(originsToAdd.Select(o => new ClientCorsOrigin
        {
            Origin = o
        }));
        
        existingClient.ClientSecrets.RemoveAll(s => !client.ClientSecrets.Select(cs => cs.Secret).Contains(s.Value));
        var secretsToAdd = client.ClientSecrets.Where(s => existingClient.ClientSecrets.All(e => e.Value != s.Secret));
        existingClient.ClientSecrets.AddRange(secretsToAdd.Select(s => new ClientSecret
        {
            Value = s.Secret,
            Description = s.Description
        }));
        
        existingClient.AllowedGrantTypes.RemoveAll(g => !client.AllowedGrantTypes.Contains(g.GrantType));
        var grantTypesToAdd = client.AllowedGrantTypes.Where(g => existingClient.AllowedGrantTypes.All(e => e.GrantType != g));
        existingClient.AllowedGrantTypes.AddRange(grantTypesToAdd.Select(g => new ClientGrantType
        {
            GrantType = g
        }));
        existingClient.RequirePkce = client.RequirePkce;
        
        existingClient.AllowedScopes.RemoveAll(s => !client.AllowedScopes.Contains(s.Scope));
        var scopesToAdd = client.AllowedScopes.Where(s => existingClient.AllowedScopes.All(e => e.Scope != s));
        existingClient.AllowedScopes.AddRange(scopesToAdd.Select(s => new ClientScope
        {
            Scope = s
        }));
        
        existingClient.AllowedCorsOrigins.RemoveAll(o => !client.AllowedCorsOrigins.Contains(o.Origin));
        var corsOriginsToAdd = client.AllowedCorsOrigins.Where(o => existingClient.AllowedCorsOrigins.All(e => e.Origin != o));
        existingClient.AllowedCorsOrigins.AddRange(corsOriginsToAdd.Select(o => new ClientCorsOrigin
        {
            Origin = o
        }));
        
        _dbContext.Clients.Update(existingClient);
        
        return _dbContext
            .SaveChangesAsync()
            .ContinueWith(_ => client);
    }

    public async Task<ClientViewModel> CreateClient(ClientViewModel client)
    {
        var newClient = new Client
        {
            ClientId = client.ClientId,
            ClientName = client.ClientName,
            Description = client.Description,
            AllowAccessTokensViaBrowser = client.AllowAccessTokensViaBrowser,
            RequireConsent = client.RequireConsent,
            AlwaysIncludeUserClaimsInIdToken = client.AlwaysIncludeUserClaimsInIdToken,
            AccessTokenLifetime = client.AccessTokenLifetime,
            ClientUri = client.ClientUri,
            RedirectUris = client.RedirectUris.Select(r => new ClientRedirectUri
            {
                RedirectUri = r
            }).ToList(),
            PostLogoutRedirectUris = client.PostLogoutRedirectUris.Select(p => new ClientPostLogoutRedirectUri
            {
                PostLogoutRedirectUri = p
            }).ToList(),
            AllowedCorsOrigins = client.AllowedCorsOrigins.Select(o => new ClientCorsOrigin
            {
                Origin = o
            }).ToList(),
            ClientSecrets = client.ClientSecrets.Select(s => new ClientSecret
            {
                Value = s.Secret,
                Description = s.Description
            }).ToList(),
            AllowedGrantTypes = client.AllowedGrantTypes.Select(g => new ClientGrantType
            {
                GrantType = g
            }).ToList(),
            RequirePkce = client.RequirePkce,
            AllowedScopes = client.AllowedScopes.Select(s => new ClientScope
            {
                Scope = s
            }).ToList()
        };

        _dbContext.Clients.Add(newClient);
        await _dbContext.SaveChangesAsync();

        client.Id = newClient.Id;
        return client;
    }
}
