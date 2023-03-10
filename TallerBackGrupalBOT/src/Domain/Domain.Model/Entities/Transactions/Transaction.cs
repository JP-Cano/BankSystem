using System;
using Domain.Model.Entities.Accounts;

namespace Domain.Model.Entities.Transactions
{
    /// <summary>
    /// Class <see cref="Transaction"/>
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Id de entidad <see cref="Account"/>
        /// </summary>
        public string AccountId { get; private set; }

        /// <summary>
        /// Movement date
        /// </summary>
        public DateTime MovementDate { get; private set; }

        /// <summary>
        /// Transaction type
        /// </summary>
        public TransactionType TransactionType { get; private set; }

        /// <summary>
        /// Value
        /// </summary>
        public decimal Value { get; private set; }

        /// <summary>
        /// Initial balance
        /// </summary>
        public decimal InitialBalance { get; private set; }

        /// <summary>
        /// Final balance
        /// </summary>
        public decimal FinalBalance { get; private set; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Crea una instancia de clase <see cref="Transaction"/> con los atributos id de entidad
        /// <see cref="Account"/> y value
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="value"></param>
        public Transaction(string accountId, decimal value)
        {
            AccountId = accountId;
            Value = value;
        }

        /// <summary>
        /// Crea una instancia de la clase <see cref="Transaction"/> sin el Id
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="movementDate"></param>
        /// <param name="transactionType"></param>
        /// <param name="value"></param>
        /// <param name="initialBalance"></param>
        /// <param name="finalBalance"></param>
        /// <param name="description"></param>
        public Transaction(string accountId, DateTime movementDate,
            TransactionType transactionType, decimal value, decimal initialBalance,
            decimal finalBalance, string description)
        {
            AccountId = accountId;
            MovementDate = movementDate;
            TransactionType = transactionType;
            Value = value;
            InitialBalance = initialBalance;
            FinalBalance = finalBalance;
            Description = description;
        }

        /// <summary>
        /// Crea una instancia de clase <see cref="Transaction"/> con todos los atributos
        /// </summary>
        /// <param name="id"></param>
        /// <param name="accountId"></param>
        /// <param name="movementDate"></param>
        /// <param name="transactionType"></param>
        /// <param name="value"></param>
        /// <param name="initialBalance"></param>
        /// <param name="finalBalance"></param>
        /// <param name="description"></param>
        public Transaction(string id, string accountId, DateTime movementDate,
            TransactionType transactionType, decimal value, decimal initialBalance,
            decimal finalBalance, string description)
        {
            Id = id;
            AccountId = accountId;
            MovementDate = movementDate;
            TransactionType = transactionType;
            Value = value;
            InitialBalance = initialBalance;
            FinalBalance = finalBalance;
            Description = description;
        }

        /// <summary>
        /// Assign <see cref="TransactionType"/>
        /// </summary>
        /// <param name="type"></param>
        public void AssignTransactionType(TransactionType type) => TransactionType = type;

        /// <summary>
        /// Assign movement date
        /// </summary>
        public void AddMovementDate() => MovementDate = DateTime.Now;

        /// <summary>
        /// Assign initial value
        /// </summary>
        /// <param name="value"></param>
        public void AssignInitialBalance(decimal value) => InitialBalance = value;

        /// <summary>
        /// Assign debt final balance
        /// </summary>
        /// <param name="value"></param>
        public void AssignDebitFinalBalance(decimal value) => FinalBalance = InitialBalance - value;

        /// <summary>
        /// Assign credit final balance
        /// </summary>
        /// <param name="value"></param>
        public void AssignFinalCreditBalance(decimal value) => FinalBalance = InitialBalance + value;

        /// <summary>
        /// Generate description
        /// </summary>
        /// <param name="receiverAccount"></param>
        public void GenerateDescription(string receiverAccount = "")
        {
            Description = TransactionType switch
            {
                TransactionType.Deposit => $"Deposit made of ${Value} to account with ID {AccountId}",

                TransactionType.Withdrawal => $"Withdrawal made of ${Value} from account with ID {AccountId}",

                TransactionType.Transfer =>
                    $"Transfer made of ${Value} from account with ID {AccountId} to account with ID {receiverAccount}",

                _ => Description
            };
        }
    }
}