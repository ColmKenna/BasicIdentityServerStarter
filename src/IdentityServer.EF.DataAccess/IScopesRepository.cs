namespace IdentityServer.EF.DataAccess;

public interface IScopesRepository
{
    Task<List<string>> GetScopes();
}