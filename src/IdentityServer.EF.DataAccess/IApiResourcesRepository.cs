using System.Collections.Generic;
using System.Threading.Tasks;
using Duende.IdentityServer.EntityFramework.Entities;

namespace IdentityServer.EF.DataAccess
{
    public interface IApiResourcesRepository
    {
        Task<List<ApiResource>> GetApiResources();
        Task<ApiResource> GetApiResourceById(int id);
        Task CreateApiResource(ApiResource apiResource);
        Task UpdateApiResource(ApiResource apiResource);
        Task DeleteApiResource(int id);
    }
}
