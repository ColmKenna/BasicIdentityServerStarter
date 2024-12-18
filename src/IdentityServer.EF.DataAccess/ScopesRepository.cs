using Duende.IdentityServer.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.EF.DataAccess;

public class ScopesRepository : IScopesRepository
{
    private readonly ConfigurationDbContext _dbContext;

    public ScopesRepository(ConfigurationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<string>> GetScopes()
    {
        var scopes = await _dbContext.ApiScopes.Select(s => s.Name).ToListAsync();
        var identityResourcesScopes = await _dbContext.IdentityResources.Select(i => i.Name).ToListAsync();
        
        return scopes.Concat(identityResourcesScopes).ToList(); 
        
    }
}