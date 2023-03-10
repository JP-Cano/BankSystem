using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Model.Entities.Accounts;

namespace Domain.UseCase.Accounts
{
    /// <summary>
    /// Interface Cuenta UseCase
    /// </summary>
    public interface ICuentaUseCase
    {
        /// <summary>
        /// Cancel an account. <param name="userId"></param><param name="account"></param>
        /// </summary>
        /// <returns></returns>
        Task<Account> CancelAccount(string userId, Account account);

        /// <summary>
        /// Enable an account <param name="userId"></param><param name="account"></param>
        /// </summary>
        /// <returns></returns>
        Task<Account> EnableAccount(string userId, Account account);

        /// <summary>
        /// Disable an account <param name="userId"></param><param name="account"></param>
        /// </summary>
        /// <returns></returns>
        Task<Account> DisableAccount(string userId, Account account);

        /// <summary>
        /// Return account by id
        /// </summary>
        /// <returns></returns>
        Task<Account> FindAccountById(string idCuenta);

        /// <summary>
        /// Create account
        /// </summary>
        /// <param name="account"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Account> Create(string userId, Account account);

        /// <summary>
        /// Return all accounts
        /// </summary>
        /// <returns></returns>
        Task<List<Account>> FindAll();

        /// <summary>
        /// Return all accounts by client id
        /// </summary>
        /// <returns></returns>
        Task<List<Account>> FindAllByClient(string clientId);
    }
}