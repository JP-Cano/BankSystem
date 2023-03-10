using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Domain.Model.Entities.Clientes;

namespace Domain.Model.Entities.Clients
{
    /// <summary>
    /// Entidad de cliente
    /// </summary>
    public class Client
    {
        /// <summary>
        /// Current date
        /// </summary>
        private readonly DateTime _currentDate = DateTime.Now;

        /// <summary>
        /// Client Id
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Id type
        /// </summary>
        public IdType IdType { get; private set; }

        /// <summary>
        /// Id number
        /// </summary>
        public string IdNumber { get; private set; }

        /// <summary>
        /// Client Names
        /// </summary>
        public string Names { get; private set; }

        /// <summary>
        /// Client LastNames
        /// </summary>
        public string LastNames { get; private set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; private set; }

        /// <summary>
        /// Birthdate
        /// </summary>
        public DateTime Birthdate { get; private set; }

        /// <summary>
        /// Client creation date
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Clint's data history updates
        /// </summary>
        public List<HistoryUpdate> HistoryUpdates { get; private set; }

        /// <summary>
        /// Client status
        /// </summary>
        public bool IsEnabled { get; private set; }

        /// <summary>
        /// If client has active debts
        /// </summary>
        public bool HasActiveDebts { get; private set; }

        /// <summary>
        /// Client's products
        /// </summary>
        public List<string> Products { get; private set; }

        /// <summary>
        /// Constructor vació
        /// </summary>
        public Client()
        {
        }

        /// <summary>
        /// Constructor cliente
        /// </summary>
        /// <param name="idType"></param>
        /// <param name="idNumber"></param>
        /// <param name="names"></param>
        /// <param name="lastNames"></param>
        /// <param name="email"></param>
        /// <param name="birthdate"></param>

        public Client(string id, IdType idType, string idNumber, string names,
            string lastNames, string email, DateTime birthdate)
        {
            Id = id;
            IdType = idType;
            IdNumber = idNumber;
            Names = names;
            LastNames = lastNames;
            Email = email;
            Birthdate = birthdate;
            CreationDate = DateTime.Now;
            HistoryUpdates = null;
            IsEnabled = true;
            HasActiveDebts = false;
            Products = null;
        }

        /// <summary>
        /// Check if client's mail is valid
        /// </summary>
        /// <returns></returns>
        public bool CheckEmail() =>
            Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);

        /// <summary>
        /// Check if client is legal age
        /// </summary>
        /// <returns></returns>
        public bool VerifyClientAge(LegalAge legalAge)
        {
            var yearCount = (Birthdate.AddYears((int)legalAge)).Year;
            return yearCount <= _currentDate.Year;
        }

        /// <summary>
        /// Disable client
        /// </summary>
        public void Disable() => IsEnabled = false;

        /// <summary>
        /// Enable client
        /// </summary>
        public void Enable() => IsEnabled = true;

        /// <summary>
        /// Enable a debt
        /// </summary>
        public void EnableDebt() => HasActiveDebts = true;

        /// <summary>
        /// Disable a debt
        /// </summary>
        public void DisableDebt() => HasActiveDebts = false;

        /// <summary>
        /// Update client's current email
        /// </summary>
        /// <param name="newEmail"></param>
        public void UpdateEmail(string newEmail) => Email = newEmail;

        /// <summary>
        /// Adds product id to a client
        /// </summary>
        /// <param name="account"></param>
        public void AddProductId(string account)
        {
            Products ??= new List<string>();
            Products.Add(account);
        }

        /// <summary>
        /// Adds new history update to client
        /// </summary>
        /// <param name="historyUpdate"></param>
        public void AddHistoryUpdate(HistoryUpdate historyUpdate)
        {
            HistoryUpdates ??= new List<HistoryUpdate>();
            HistoryUpdates.Add(historyUpdate);
        }
    }
}