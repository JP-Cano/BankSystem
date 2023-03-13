using Domain.Model.Entities.Accounts;

namespace EntryPoints.ReactiveWeb.Entities.Commands
{
    /// <summary>
    /// Comando para crear una entidad de tipo <see cref="Account"/>
    /// </summary>
    public class CreateAccount
    {
        /// <summary>
        /// Id del cliente
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Tipo de cuenta
        /// </summary>
        public AccountType AccountType { get; set; }

        /// <summary>
        /// Balance de la cuenta
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        /// Indica si la cuenta es exenta
        /// </summary>
        public bool Exempt { get; set; }
    }
}