using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Model.Entities.Clients;

namespace Domain.Model.Entities.Gateway
{
    /// <summary>
    /// Client repository
    /// </summary>
    public interface IClientRepository
    {
        /// <summary>
        /// Return all clients
        /// </summary>
        /// <returns></returns>
        Task<List<Client>> FindAllAsync();

        /// <summary>
        /// Return a client by id
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        Task<Client> FindByIdAsync(string clientId);

        /// <summary>
        /// Return a client by id number
        /// </summary>
        /// <param name="idNumber"></param>
        /// <returns></returns>
        Task<Client> FindByIdNumber(string idNumber);

        /// <summary>
        /// CreateAsync a new client
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        Task<Client> CreateAsync(string userId, Client client);

        /// <summary>
        /// Update client by id
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        Task<Client> UpdateAsync(string clientId, Client client);
    }
}