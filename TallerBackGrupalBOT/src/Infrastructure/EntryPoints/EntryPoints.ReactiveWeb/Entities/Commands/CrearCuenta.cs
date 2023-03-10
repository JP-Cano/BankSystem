using Domain.Model.Entities.Accounts;

namespace EntryPoints.ReactiveWeb.Entities.Commands
{
    /// <summary>
    /// Comando para crear una entidad de tipo <see cref="Account"/>
    /// </summary>
    public class CrearCuenta
    {
        /// <summary>
        /// Id del cliente
        /// </summary>
        public string IdCliente { get; set; }

        /// <summary>
        /// Tipo de cuenta
        /// </summary>
        public AccountType AccountType { get; set; }

        /// <summary>
        /// Balance de la cuenta
        /// </summary>
        public decimal Saldo { get; set; }

        /// <summary>
        /// Indica si la cuenta es exenta
        /// </summary>
        public bool Exenta { get; set; }
    }
}