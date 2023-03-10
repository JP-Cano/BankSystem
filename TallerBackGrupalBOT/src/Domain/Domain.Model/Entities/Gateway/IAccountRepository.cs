using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Model.Entities.Accounts;

namespace Domain.Model.Entities.Gateway
{
    /// <summary>
    /// <see cref="Account"/> repository interface
    /// </summary>
    public interface IAccountRepository
    {
        /// <summary>
        /// Returns all accounts
        /// </summary>
        /// <returns></returns>
        Task<List<Account>> FindAllAsync();

        /// <summary>
        /// Return account by id
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<Account> FindByIdAsync(string accountId);

        /// <summary>
        /// CreateAsync an account
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        Task<Account> CreateAsync(Account account);

        /// <summary>
        /// Update an account by id
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        Task<Account> UpdateAsync(string accountId, Account account);

        /// <summary>
        /// Return account by client id
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        Task<List<Account>> FindByClientAsync(string clientId);
    }
}