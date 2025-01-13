using IdentityDataModels;

namespace IdentityServer.EF.DataAccess;

public interface IRolesRepository   
{
    Task<List<Role>> GetRoles();
    Task<Role?> GetRoleById(string id);
    Task<Role?> GetRoleByName(string name);
    Task<List<ApplicationUserSummary>> GetUsersInRole(string roleName);
    
    Task<Role?> UpdateRole(Role role);
    Task<Role?> CreateRole(Role role);

    Task<bool> DeleteRole(string id);
    
    Task<bool> DeleteRoleByName(string id);
    
}