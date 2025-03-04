﻿using IdentityDataModels;
using IdentityServer.EF.DataAccess.DataMigrations;

namespace IdentityServer.EF.DataAccess;

public interface IClientsRepository
{
    public Task<List<ClientsSummary>> GetClients();
    Task<ClientViewModel?> GetClient(string clientId);
    Task<ClientViewModel> UpdateClient(ClientViewModel client);
    Task<ClientViewModel> CreateClient(ClientViewModel client);
}
