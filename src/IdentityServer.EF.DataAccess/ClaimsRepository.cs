using IdentityServer.EF.DataAccess.DataMigrations;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.EF.DataAccess;

public class ClaimsRepository : IClaimsRepository
{
    private ApplicationDbContext applicationDbContext;

    public ClaimsRepository(ApplicationDbContext applicationDbContext)
    {
        this.applicationDbContext = applicationDbContext;
    }

    public Task<List<UserClaim>> GetClaims()
    {
        return applicationDbContext.UserClaims.Select(uc => new UserClaim
            {
                ClaimType = uc.ClaimType,
                ClaimValue = ""
            }).Distinct()
            .ToListAsync();
    }
}