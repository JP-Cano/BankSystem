using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Model.Entities.Accounts;
using Domain.Model.Entities.Transactions;

namespace Domain.Model.Entities.Gateway
{
    /// <summary>
    /// Transaction repository interface
    /// </summary>
    public interface ITransactionRepository
    {
        /// <summary>
        /// Return a transaction by id
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        Task<Transaction> FindByIdAsync(string transactionId);

        /// <summary>
        /// Return transactions by account id
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<List<Transaction>> FindByAccountIdAsync(string accountId);

        /// <summary>
        /// CreateAsync a transaction
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        Task<Transaction> CreateAsync(Transaction transaction);
    }
}