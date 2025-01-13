using System.Collections.Generic;
using System.Threading.Tasks;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.EF.DataAccess
{
    public class ApiResourcesRepository : IApiResourcesRepository
    {
        private readonly ConfigurationDbContext _dbContext;

        public ApiResourcesRepository(ConfigurationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ApiResource>> GetApiResources()
        {
            return await _dbContext.ApiResources.ToListAsync();
        }

        public async Task<ApiResource> GetApiResourceById(int id)
        {
            return await _dbContext.ApiResources.FindAsync(id);
        }

        public async Task CreateApiResource(ApiResource apiResource)
        {
            _dbContext.ApiResources.Add(apiResource);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateApiResource(ApiResource apiResource)
        {
            _dbContext.ApiResources.Update(apiResource);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteApiResource(int id)
        {
            var apiResource = await _dbContext.ApiResources.FindAsync(id);
            if (apiResource != null)
            {
                _dbContext.ApiResources.Remove(apiResource);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
