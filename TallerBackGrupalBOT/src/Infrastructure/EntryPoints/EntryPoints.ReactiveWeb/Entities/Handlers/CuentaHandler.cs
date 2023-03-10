using Domain.Model.Entities.Clientes;
using System.Collections.Generic;
using Domain.Model.Entities.Accounts;
using Domain.Model.Entities.Clients;
using Domain.Model.Entities.Transactions;

namespace EntryPoints.ReactiveWeb.Entities.Handlers
{
    /// <summary>
    /// Handler DTO de entidad <see cref="Account"/>
    /// </summary>
    public class CuentaHandler
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Id de entidad <see cref="Client"/>
        /// </summary>
        public string IdCliente { get; set; }

        /// <summary>
        /// Numero de cuenta
        /// </summary>
        public string NumeroCuenta { get; set; }

        /// <summary>
        /// Tipo de cuenta
        /// </summary>
        public AccountType AccountType { get; set; }

        /// <summary>
        /// Estado de cuenta
        /// </summary>
        public AccountStatus AccountStatus { get; set; }

        /// <summary>
        /// Balance
        /// </summary>
        public decimal Saldo { get; set; }

        /// <summary>
        /// Balance disponible
        /// </summary>
        public decimal SaldoDisponible { get; set; }

        /// <summary>
        /// Exempt
        /// </summary>
        public bool Exenta { get; set; }

        /// <summary>
        /// Lista de tipo <see cref="Transaction"/>
        /// </summary>
        public List<TransacciónHandler> Transacciones { get; set; }
    }
}