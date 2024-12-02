using System.Linq.Expressions;
using IdentityDataModels;
using IdentityServer.EF.DataAccess.DataMigrations;
using IdentityServerAspNetIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.EF.DataAccess;

public class UserRepository : IUserRepository
{
    private ApplicationDbContext applicationDbContext;
    private UserManager<ApplicationUser> userManager;

    public UserRepository(ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager)
    {
        this.applicationDbContext = applicationDbContext;
        this.userManager = userManager;
    }


    public  Task<List<ApplicationUserSummary>> GetUsers()
    {
        return applicationDbContext.Users.GroupJoin(applicationDbContext.UserClaims, user => user.Id,
            identityUserClaim => identityUserClaim.UserId,
            (user, identityUserClaims) => new ApplicationUserSummary
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                UserClaims = identityUserClaims.Select(x => x.ClaimType).ToList()
            }).ToListAsync();
    }

    
    private IQueryable<ApplicationUserDetails> getUserIQueryable(Expression<Func<ApplicationUser, bool>> predicate)
    {
        return applicationDbContext.Users
            .Where(predicate) // Ensures predicate is properly converted and used in SQL
            .Select(user => new ApplicationUserDetails
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                LockoutEnd = user.LockoutEnd,
                UserClaims = applicationDbContext.UserClaims
                    .Where(uc => uc.UserId == user.Id)
                    .Select(uc => new UserClaimSummary
                    {
                        Id = uc.Id, 
                        UserId = uc.UserId, 
                        ClaimType = uc.ClaimType, 
                        ClaimValue = uc.ClaimValue
                    }).ToList(),
                Roles = applicationDbContext.UserRoles
                    .Where(ur => ur.UserId == user.Id)
                    .Join(applicationDbContext.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
                    .ToList()
            });

        
    }

    private Task<ApplicationUserDetails?> GetUser(Expression<Func<ApplicationUser, bool>> predicate) => getUserIQueryable(predicate).FirstOrDefaultAsync();

    public Task<ApplicationUserDetails?> GetUserById(string id) => GetUser(user => user.Id == id);

    public Task<ApplicationUserDetails?> GetUserByUserName(string userName) => GetUser(user => user.UserName == userName);

    public Task<ApplicationUserDetails?> GetUserByEmail(string email) => GetUser(user => user.Email == email);
    
    public Task<ApplicationUserDetails?> GetUserByPhoneNumber(string phoneNumber) => GetUser(user => user.PhoneNumber == phoneNumber);
    public Task<bool> AddClaimToUser(string userId, UserClaim claim)
    {
        // check if claim already exists
        var existingUserClaim = applicationDbContext.UserClaims.FirstOrDefault(uc => uc.UserId == userId && uc.ClaimType == claim.ClaimType && uc.ClaimValue == claim.ClaimValue);
        if (existingUserClaim != null)
        {
            return Task.FromResult(false);
        }
        
        // check if user exists
        var user = applicationDbContext.Users.FirstOrDefault(u => u.Id == userId);
        if (user == null)
        {
            return Task.FromResult(false);
        } 
        
        var userClaim = new IdentityUserClaim<string>
        {
            UserId = userId,
            ClaimType = claim.ClaimType,
            ClaimValue = claim.ClaimValue
        };
        applicationDbContext.UserClaims.Add(userClaim);
        
        
        return applicationDbContext.SaveChangesAsync().ContinueWith(t => t.Result > 0);
    }
    
    public Task<bool> RemoveClaimFromUser(string userId, UserClaim claim)
    {
        var userClaim = applicationDbContext.UserClaims.FirstOrDefault(uc => uc.UserId == userId && uc.ClaimType == claim.ClaimType && uc.ClaimValue == claim.ClaimValue);
        if (userClaim == null)
        {
            return Task.FromResult(false);
        }
        applicationDbContext.UserClaims.Remove(userClaim);
        return applicationDbContext.SaveChangesAsync().ContinueWith(t => t.Result > 0);
    }
    
    public Task<bool> AddClaimsToUser(string userId, List<UserClaim> claims)
    {
        var user = applicationDbContext.Users.FirstOrDefault(u => u.Id == userId);
        if (user == null)
        {
            return Task.FromResult(false);
        }
        
        var userClaims = claims.Select(claim => new IdentityUserClaim<string>
        {
            UserId = userId,
            ClaimType = claim.ClaimType,
            ClaimValue = claim.ClaimValue
        }).ToList();
        userClaims = userClaims.Where(uc => !applicationDbContext.UserClaims.Any(uc2 => uc2.UserId == uc.UserId && uc2.ClaimType == uc.ClaimType && uc2.ClaimValue == uc.ClaimValue)).ToList();
        applicationDbContext.UserClaims.AddRange(userClaims);
        return applicationDbContext.SaveChangesAsync().ContinueWith(t => t.Result > 0);
    }
    
    public async Task<bool> UpdateUserClaims(string userId, List<UserClaim> claims)
    {
        var user = applicationDbContext.Users.FirstOrDefault(u => u.Id == userId);
        if (user == null)
        {
            return false;
        }

        var cl =await  userManager.GetClaimsAsync(user);
        
        var userClaims = applicationDbContext.UserClaims.Where(uc => uc.UserId == userId ).ToList();
        
        var newUserClaims = claims.Select(claim => new IdentityUserClaim<string>
        {
            UserId = userId,
            ClaimType = claim.ClaimType,
            ClaimValue = claim.ClaimValue
        }).ToList();
//        applicationDbContext.UserClaims.AddRange(newUserClaims);
        var claimsToRemove = userClaims.Where(uc => !newUserClaims.Any(uc2 => uc2.ClaimType == uc.ClaimType)).ToList();
        var claimsToAdd = newUserClaims.Where(uc => !userClaims.Any(uc2 => uc2.ClaimType == uc.ClaimType)).ToList();
        var claimsToUpdate = newUserClaims.Where(uc => userClaims.Any(uc2 => uc2.ClaimType == uc.ClaimType && uc2.ClaimValue != uc.ClaimValue)).ToList();
        
        applicationDbContext.UserClaims.RemoveRange(claimsToRemove);
        applicationDbContext.UserClaims.AddRange(claimsToAdd);
        foreach (var claim in claimsToUpdate)
        {
            var userClaim = userClaims.FirstOrDefault(uc => uc.ClaimType == claim.ClaimType);
            if (userClaim != null)
            {
                userClaim.ClaimValue = claim.ClaimValue;
            }
        }
        
        
        return await applicationDbContext.SaveChangesAsync().ContinueWith(t => t.Result > 0);
    }
    
    
    public async Task<bool> RemoveClaimsFromUser(string userId, List<UserClaim> claims)
    {
        var a = await userManager.GetClaimsAsync(await userManager.FindByIdAsync(userId));
       
       
        var user = await applicationDbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            return false;
        }
        
        var userClaims = applicationDbContext.UserClaims.Where(uc => uc.UserId == userId && claims.Any(c => c.ClaimType == uc.ClaimType && c.ClaimValue == uc.ClaimValue)).ToList();
        applicationDbContext.UserClaims.RemoveRange(userClaims);
        return await applicationDbContext.SaveChangesAsync().ContinueWith(t => t.Result > 0);
    }
    
    public Task<bool> AddUserToRole(string userId, string roleName)
    {
        var user = applicationDbContext.Users.FirstOrDefault(u => u.Id == userId);
        if (user == null)
        {
            return Task.FromResult(false);
        }
        
        var role = applicationDbContext.Roles.FirstOrDefault(r => r.Name == roleName);
        if (role == null)
        {
            return Task.FromResult(false);
        }
        
        var userRole = new IdentityUserRole<string>
        {
            UserId = userId,
            RoleId = role.Id
        };
        applicationDbContext.UserRoles.Add(userRole);
        return applicationDbContext.SaveChangesAsync().ContinueWith(t => t.Result > 0);
    }
    
    public Task<bool> RemoveUserFromRole(string userId, string roleName)
    {
        var userRole = applicationDbContext.UserRoles
            .Where(ur => ur.UserId == userId)
            .Join(applicationDbContext.Roles, userRole => userRole.RoleId, role => role.Id, (userRole, role) => new { userRole, role })
            .FirstOrDefault(joinResult => joinResult.role.Name == roleName)
            ?.userRole;
        
        if (userRole == null)
        {
            return Task.FromResult(false);
        }
        applicationDbContext.UserRoles.Remove(userRole);
        return applicationDbContext.SaveChangesAsync().ContinueWith(t => t.Result > 0);
    }
    
    public async Task<bool> UpdateUserRoles(string userId, List<string> roles)
    {
        var user = applicationDbContext.Users.FirstOrDefault(u => u.Id == userId);
        if (user == null)
        {
            return false;
        }
        
        var existingUserRoles = applicationDbContext.UserRoles.Where(ur => ur.UserId == userId).ToList();
        var allRoles = applicationDbContext.Roles.Where(r => roles.Contains(r.Name)).ToList();
        var newRolesa = roles.Where(roleName => allRoles.All(r => r.Name != roleName)).Select(roleName => new IdentityRole
        {
            Name = roleName,
            NormalizedName = roleName.ToUpper()
        }).ToList();
        applicationDbContext.Roles.AddRange(newRolesa);
        await applicationDbContext.SaveChangesAsync().ConfigureAwait(false);
        allRoles = await applicationDbContext.Roles.Where(r => roles.Contains(r.Name)).ToListAsync();
        

        var updatedUserRoles = roles.Select(roleName => new IdentityUserRole<string>
        {
            UserId = userId,
            RoleId = allRoles.FirstOrDefault(r => r.Name == roleName)?.Id
        }).ToList();
        
        var newRoles = updatedUserRoles.Where(ur => !existingUserRoles.Any(er => er.RoleId == ur.RoleId)).ToList();
        var removedRoles = existingUserRoles.Where(er => !updatedUserRoles.Any(ur => ur.RoleId == er.RoleId)).ToList();
        
        applicationDbContext.UserRoles.RemoveRange(removedRoles);
        applicationDbContext.UserRoles.AddRange(newRoles);
        
        
        return await applicationDbContext.SaveChangesAsync().ContinueWith(t => t.Result > 0);
    }
    
    public async Task<bool> UpdateUser(ApplicationUserDetails user)
    {
        var userEntity = applicationDbContext.Users.FirstOrDefault(u => u.Id == user.Id);
        if (userEntity == null)
        {
            return false;
        }

        userEntity.UserName = user.UserName;
        userEntity.Email = user.Email;
        userEntity.PhoneNumber = user.PhoneNumber;
        userEntity.LockoutEnd = user.LockoutEnd;
        userEntity.TwoFactorEnabled = user.TwoFactorEnabled;
        userEntity.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
        userEntity.LockoutEnabled = user.LockoutEnabled;
        userEntity.AccessFailedCount = user.AccessFailedCount;
        
        
        
        
        
        await UpdateUserRoles(user.Id, user.Roles.ToList()).ConfigureAwait(false);
        await UpdateUserClaims(user.Id, user.UserClaims.Select(uc => new UserClaim { ClaimType = uc.ClaimType, ClaimValue = uc.ClaimValue }).ToList()).ConfigureAwait(false);
        await applicationDbContext.SaveChangesAsync().ConfigureAwait(false);

        return true;
    }
    
   
    public async Task<bool> LockUser(string userId, DateTimeOffset lockoutEnd)
    {
        var user = applicationDbContext.Users.FirstOrDefault(u => u.Id == userId);
        if (user == null)
        {
            return false;
        }
        user.LockoutEnd = lockoutEnd;
        await applicationDbContext.SaveChangesAsync().ContinueWith(t => t.Result > 0).ConfigureAwait(false);
        return true;
    }
    
    public Task<bool> LockUser(string userId)
    {
        return LockUser(userId, DateTimeOffset.MaxValue);
    }
    
    public async Task<bool> UnlockUser(string userId)
    {
        var user = applicationDbContext.Users.FirstOrDefault(u => u.Id == userId);
        if (user == null)
        {
            return false;
        }
        user.LockoutEnd = null;
        await applicationDbContext.SaveChangesAsync();
        return true;
    }
    
    
      
}