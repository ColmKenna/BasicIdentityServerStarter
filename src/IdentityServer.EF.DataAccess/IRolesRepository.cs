using IdentityDataModels;

namespace IdentityServer.EF.DataAccess;

public interface IRolesRepository   
{
    Task<List<Role>> GetRoles();
    Task<Role?> GetRoleById(string id);
    Task<Role?> GetRoleByName(string name);
    Task<List<ApplicationUserSummary>> GetUsersInRole(string roleName);
}