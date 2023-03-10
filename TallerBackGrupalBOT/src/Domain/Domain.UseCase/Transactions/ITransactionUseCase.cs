using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Model.Entities.Accounts;
using Domain.Model.Entities.Transactions;

namespace Domain.UseCase.Transactions
{
    /// <summary>
    /// Interfaz de caso de uso de entidad <see cref="Transaction"/>
    /// </summary>
    public interface ITransactionUseCase
    {
        /// <summary>
        /// Método para obtener una entidad de tipo <see cref="Transaction"/> por su Id
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        Task<Transaction> FindTransactionById(string transactionId);

        /// <summary>
        /// Método para obtener una lista de entidad <see cref="Transaction"/> por el Id de tipo
        /// <see cref="Account"/>
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<List<Transaction>> FindTransactionsByAccountId(string accountId);

        /// <summary>
        /// Método para realizar una consignación
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        Task<Transaction> MakeDeposit(Transaction transaction);

        /// <summary>
        /// Método para realizar un retiro
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        Task<Transaction> MakeWithdrawal(Transaction transaction);

        /// <summary>
        /// Método para realizar una transferencia
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="idCuentaReceptor"></param>
        /// <returns></returns>
        Task<Transaction> MakeTransfer(Transaction transaction, string idCuentaReceptor);
    }
}