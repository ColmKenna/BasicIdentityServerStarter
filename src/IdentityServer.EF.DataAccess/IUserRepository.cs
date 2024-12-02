using IdentityDataModels;

namespace IdentityServer.EF.DataAccess;

public interface IUserRepository
{
    Task<List<ApplicationUserSummary>> GetUsers();
    Task<ApplicationUserDetails?> GetUserById(string id);
    Task<ApplicationUserDetails?> GetUserByUserName(string userName);
    Task<ApplicationUserDetails?> GetUserByEmail(string email);
    Task<ApplicationUserDetails?> GetUserByPhoneNumber(string phoneNumber);
    
    // add user to role
    Task<bool> AddUserToRole(string userId, string roleName);
    
    // remove user from role
    Task<bool> RemoveUserFromRole(string userId, string roleName);
    
    // add claim to user
    Task<bool> AddClaimToUser(string userId, UserClaim claim);
    
    // remove claim from user
    Task<bool> RemoveClaimFromUser(string userId, UserClaim claim);
    
    // add claims to user
    Task<bool> AddClaimsToUser(string userId, List<UserClaim> claims);

    
    // remove claims from user
    Task<bool> RemoveClaimsFromUser(string userId, List<UserClaim> claims);
    
    // Update User claims
    Task<bool> UpdateUserClaims(string userId, List<UserClaim> claims);
    
    Task<bool> UpdateUserRoles(string userId, List<string> roles); 
    
    // update User
    Task<bool> UpdateUser(ApplicationUserDetails user);
    
    // Mark user as Locked, give a lockout end date longer than now to lock the user indefinitely
    
    Task<bool> LockUser(string userId);
    Task<bool> LockUser(string userId, DateTimeOffset lockoutEnd);
    
    Task<bool> UnlockUser(string userId);
    
}