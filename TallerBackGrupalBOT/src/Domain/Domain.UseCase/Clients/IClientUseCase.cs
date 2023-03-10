using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Model.Entities.Accounts;
using Domain.Model.Entities.Clients;

namespace Domain.UseCase.Clients
{
    /// <summary>
    /// Caso de uso de client
    /// </summary>
    public interface IClientUseCase
    {
        /// <summary>
        /// Create client
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newClient"></param>
        /// <returns></returns>
        Task<Client> CreateClient(string userId, Client newClient);

        /// <summary>
        /// Update email
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="newEmail"></param>
        /// <returns></returns>
        Task<Client> UpdateEmail(string clientId, string newEmail);

        /// <summary>
        /// Return client by id
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        Task<Client> FindClientById(string clientId);

        /// <summary>
        /// Enable client
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        Task<bool> EnableClient(string clientId);

        /// <summary>
        /// Disable client
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        Task<bool> DisableClient(string clientId);

        /// <summary>
        /// Add product to client
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="newAccount"></param>
        /// <returns></returns>
        Task<Client> AddProductToClient(string clientId, Account newAccount);

        /// <summary>
        /// Enable client debt
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        Task<bool> EnableClientDebt(string clientId);

        /// <summary>
        /// Disable client debt
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        Task<bool> DisableClientDebt(string clientId);

        /// <summary>
        /// Return all clients
        /// </summary>
        /// <returns></returns>
        Task<List<Client>> FindAll();
    }
}