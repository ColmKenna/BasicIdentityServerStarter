using System.Linq.Expressions;
using IdentityDataModels;
using IdentityServer.EF.DataAccess.DataMigrations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.EF.DataAccess;

public class RolesRepository : IRolesRepository
{
    private ApplicationDbContext applicationDbContext;

    public RolesRepository(ApplicationDbContext applicationDbContext)
    {
        this.applicationDbContext = applicationDbContext;
    }

    private Expression< Func<IdentityRole<string>, Role>> mapToRole = role => new Role
    {
        Id = role.Id,
        Name = role.Name
    };
    public Task<List<Role>> GetRoles()
    {
        return applicationDbContext.Roles.Select(mapToRole).ToListAsync();
    }

    public Task<Role?> GetRoleById(string id)
    {
        return applicationDbContext.Roles.Select(mapToRole).FirstOrDefaultAsync(role => role.Id == id);
    }

    public Task<Role?> GetRoleByName(string name)
    {
        return applicationDbContext.Roles.Select(mapToRole).FirstOrDefaultAsync(role => role.Name == name);
    }
    
    
    public async Task<List<ApplicationUserSummary>> GetUsersInRole(string roleName)
    {
        var userSummaries = await applicationDbContext.UserRoles
            .Join(applicationDbContext.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => new { ur, r })
            .Where(joined => joined.r.Name == roleName) // Filter by role name
            .Join(applicationDbContext.Users, joined => joined.ur.UserId, u => u.Id, (joined, u) => new { joined, u })
            .GroupJoin(
                applicationDbContext.UserClaims, 
                joined => joined.u.Id, 
                uc => uc.UserId, 
                (joined, claims) => new { joined.u, joined.joined.r, Claims = claims }
            )
            .Select(result => new ApplicationUserSummary
            {
                Id = result.u.Id,
                UserName = result.u.UserName ?? string.Empty,
                Email = result.u.Email ?? string.Empty,
                PhoneNumber = result.u.PhoneNumber ?? string.Empty,
                UserClaims = result.Claims.Select(claim => claim.ClaimType + ":" + claim.ClaimValue).ToList()
            })
            .ToListAsync();

        return userSummaries;
    }

    
    
    
    // update a role
    public async Task<Role?> UpdateRole(Role role)
    {
        var roleEntity = await applicationDbContext.Roles.FirstOrDefaultAsync(r => r.Id == role.Id);
        if (roleEntity == null)
        {
            return null;
        }

        roleEntity.Name = role.Name;
        await applicationDbContext.SaveChangesAsync().ConfigureAwait(false);
        return role;
    }
    
    // create a role
    public async Task<Role?> CreateRole(Role role)
    {
        var roleEntity = new IdentityRole
        {
            Name = role.Name,
            NormalizedName = role.Name.ToUpper()
            
        };
        await applicationDbContext.Roles.AddAsync(roleEntity).ConfigureAwait(false);
        await applicationDbContext.SaveChangesAsync().ConfigureAwait(false);
        return new Role
        {
            Id = roleEntity.Id,
            Name = roleEntity.Name
        };
    }
    
    
    // delete a role
    public async Task<bool> DeleteRole(string id)
    {
        
        var roleEntity = await applicationDbContext.Roles.FirstOrDefaultAsync(r => r.Id == id);
        if (roleEntity == null)
        {
            return false;
        }

        // check if role is used by any user
        var userRole = await applicationDbContext.UserRoles.FirstOrDefaultAsync(ur => ur.RoleId == id);
        if (userRole != null)
        {
            return false;
        }
        
        // check if role is used by any role claim
        var roleClaim = await applicationDbContext.RoleClaims.FirstOrDefaultAsync(rc => rc.RoleId == id);
        if (roleClaim != null)
        {
            return false;
        }

        applicationDbContext.Roles.Remove(roleEntity);
        await applicationDbContext.SaveChangesAsync();
        return true;
    }
}