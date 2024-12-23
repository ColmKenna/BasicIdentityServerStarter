﻿using System.Linq.Expressions;
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