using System;
using System.Collections.Generic;

namespace Domain.Model.Entities.Accounts
{
    /// <summary>
    /// Account class
    /// </summary>
    public class Account
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// ClientId
        /// </summary>
        public string ClientId { get; private set; }

        /// <summary>
        /// AccountNumber
        /// </summary>
        public string AccountNumber { get; private set; }

        /// <summary>
        /// AccountType
        /// </summary>
        public AccountType AccountType { get; private set; }

        /// <summary>
        /// AccountStatus
        /// </summary>
        public AccountStatus AccountStatus { get; private set; }

        /// <summary>
        /// Balance
        /// </summary>
        public decimal Balance { get; private set; }

        /// <summary>
        /// AvailableBalance
        /// </summary>
        public decimal AvailableBalance { get; set; }

        /// <summary>
        /// Exempt
        /// </summary>
        public bool Exempt { get; private set; }

        /// <summary>
        /// ModificationHistory
        /// </summary>
        public List<Modification> ModificationHistory { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id"></param>
        public Account(string id)
        {
            Id = id;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="clientId"></param>
        /// <param name="accountNumber"></param>
        /// <param name="accountType"></param>
        /// <param name="balance"></param>
        /// <param name="availableBalance"></param>
        /// <param name="exempt"></param>
        public Account(string id, string clientId, string accountNumber, AccountType accountType, decimal balance, decimal availableBalance, bool exempt)
        {
            Id = id;
            ClientId = clientId;
            AccountNumber = accountNumber;
            AccountType = accountType;
            AccountStatus = AccountStatus.Active;
            Balance = balance;
            AvailableBalance = availableBalance;
            Exempt = exempt;
            ModificationHistory = new List<Modification>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="accountType"></param>
        /// <param name="balance"></param>
        /// <param name="exempt"></param>
        public Account(string clientId, AccountType accountType, decimal balance, bool exempt)
        {
            ClientId = clientId;
            AccountType = accountType;
            AccountStatus = AccountStatus.Active;
            Balance = balance;
            Exempt = exempt;
            ModificationHistory = new List<Modification>();
        }

        /// <summary>
        /// Assign initial balance
        /// </summary>
        public void AssignInitialBalance(decimal initialBalance) => Balance = initialBalance;

        /// <summary>
        /// Computes available balance
        /// </summary>
        public void CalculateAvailableBalance(decimal GMF) => AvailableBalance = Balance - (Balance * GMF);

        /// <summary>
        /// Generate and Assign account number
        /// </summary>
        public void AssignAccountNumber()
        {
            var random = new Random();
            int randomNumber = random.Next(10000000, 99999999);
            if (AccountType.Equals(AccountType.Regular))
            {
                AccountNumber = $"23-{randomNumber}";
            }
            else if (AccountType.Equals(AccountType.Savings))
            {
                AccountNumber = $"46-{randomNumber}";
            }
        }

        /// <summary>
        /// Set account as exempt
        /// </summary>
        public void SetAccountAsExempt() => Exempt = true;

        /// <summary>
        /// Add new modification
        /// </summary>
        /// <param name="newModification"></param>
        public void AddModification(Modification newModification)
        {
            ModificationHistory.Add(newModification);
        }

        /// <summary>
        /// Updates the balance
        /// </summary>
        /// <param name="newBalance"></param>
        public void UpdateBalance(decimal newBalance)
        {
            Balance = newBalance;
        }

        /// <summary>
        /// Check if account is active
        /// </summary>
        public bool IsActive() => AccountStatus.Equals(AccountStatus.Active);

        /// <summary>
        /// Check if account is inactive
        /// </summary>
        public bool IsInactive() => AccountStatus.Equals(AccountStatus.Inactive);

        /// <summary>
        /// Check if account is cancelled
        /// </summary>
        public bool IsCancelled() => AccountStatus.Equals(AccountStatus.Cancelled);

        /// <summary>
        /// Enable account
        /// </summary>
        public void EnableAccount() => AccountStatus = AccountStatus.Active;

        /// <summary>
        /// Disable an account.
        /// </summary>
        public void DisableAccount() => AccountStatus = AccountStatus.Inactive;

        /// <summary>
        /// Cancel an account.
        /// </summary>
        public void CancelAccount() => AccountStatus = AccountStatus.Cancelled;
    }
}