namespace IdentityServer.EF.DataAccess;

public interface IClaimsRepository
{
    Task<List<UserClaim>> GetClaims();
}