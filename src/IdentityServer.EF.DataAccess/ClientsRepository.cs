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

    public ClientsRepository(ConfigurationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

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
        AllowAccessTokensViaBrowser = c.AllowAccessTokensViaBrowser,
        RequireConsent = c.RequireConsent,
        AlwaysIncludeUserClaimsInIdToken = c.AlwaysIncludeUserClaimsInIdToken,
        AccessTokenLifetime = c.AccessTokenLifetime,
        AllowedScopes = c.AllowedScopes.Select(s => s.Scope).ToList(),
        Id = c.Id,
        RedirectUris = c.RedirectUris.Select(r => r.RedirectUri).ToList(),
        PostLogoutRedirectUris = c.PostLogoutRedirectUris.Select(p => p.PostLogoutRedirectUri).ToList(),   
        AllowedCorsOrigins = c.AllowedCorsOrigins.Select(o => o.Origin).ToList(),
        ClientUri = c.ClientUri
    };

    public Task<ClientViewModel?> GetClient(string clientId)
    {
        return _dbContext.Clients
            .Include(c => c.AllowedGrantTypes)
            .Include(c => c.AllowedScopes)
            .Include(c => c.RedirectUris)
            .Where(c => c.ClientId == clientId)
            .Select(clientToClientViewModel)
            .FirstOrDefaultAsync();
    }
}